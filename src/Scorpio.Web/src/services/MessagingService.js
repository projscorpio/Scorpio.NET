import * as SignalR from "@microsoft/signalr";
import { MessagePackHubProtocol } from "@microsoft/signalr-protocol-msgpack";
import { API } from "../constants/appConstants";
import LogService from "./LogService";

class MessagingService {
  constructor() {
    this._connection = null;
    this._watchdogInterval = null;
    this._subsQueue = [];
    this._connectionStateObservers = [];
    this._everBeenConnected = false;
  }

  subscribeConnectionChange = handler => this._connectionStateObservers.push(handler);
  unSubscribeConnectionChange = handler => (this._connectionStateObservers = this._connectionStateObservers.filter(o => o !== handler));

  isConnected = () => this._connection && this._connection.connectionState === "Connected";

  // Subscribe for given topic. If any messages appear on it, the handler will be called with received data.
  subscribe(topic, handler) {
    if (typeof handler !== "function" || typeof topic !== "string") return;

    if (this.isConnected()) {
      LogService.info(`SignalR: subscribed to ${topic}`);
      this._connection.on(topic, handler);
    } else {
      LogService.warn(
        `Trying to subscribe to topic ${topic}, but current signalR state is: ${
          this._connection ? this._connection.connectionState : "NULL"
        }, queuing subscription`
      );

      this._subsQueue.push({ topic: topic, handler: handler });
    }
  }

  unsubscribe(topic, handler) {
    if (typeof handler !== "function" || typeof topic !== "string") return;

    if (this._connection) {
      LogService.info(`SignalR: unsubscribed ${topic}`);
      this._connection.off(topic, handler);
    }
  }

  // Send message to given topic.
  send(topic, message) {
    if (!this.isConnected()) {
      LogService.error("SignalR: send message request, bot state is not connected");
      return;
    }

    LogService.debug("Sending to topic: " + topic, message);
    this._connection.send(topic, message);
  }

  // Starts the connection. Returns promise (awaitable).
  async connectAsync() {
    if (this.isConnected()) {
      LogService.info("SignalR: trying to connect, but already connected, abadoning.");
      return;
    }

    const endpoint = API.SIGNALR;
    this._connection = new SignalR.HubConnectionBuilder()
      .withUrl(endpoint)
      .withHubProtocol(new MessagePackHubProtocol())
      .configureLogging(SignalR.LogLevel.Information)
      .withAutomaticReconnect([0, 1000, 3000])
      .build();

    window.scorpioMessaging.signalR = this._connection;

    try {
      this._setup();
      await this._connection.start();
      this._onConnected();
    } catch (err) {
      this._errorHandler(err);
    }
  }

  _onConnected = () => {
    this._everBeenConnected = true;
    LogService.info("SignalR connected!");
    this._processPendingSubsciptions();
    this._notifyConnectionStateChanged();
  };

  _processPendingSubsciptions = () => {
    if (this.isConnected() && this._subsQueue.length > 0) {
      LogService.debug("Processing pending subscriptions...", this._subsQueue);
      for (let i = 0; i <= this._subsQueue.length; i++) {
        const sub = this._subsQueue.pop();
        this.subscribe(sub.topic, sub.handler);
      }
    }
  };

  _notifyConnectionStateChanged = () => {
    if (Array.isArray(this._connectionStateObservers) && this._connectionStateObservers.length > 0 && this._connection) {
      this._connectionStateObservers.forEach(notify => {
        notify(this._connection.connectionState);
      });
    }
  };

  // Stops the connection with server.
  disconnect() {
    this._connection.stop();
    window.scorpioMessaging.signalR = null;
    if (this._watchdogInterval) {
      clearInterval(this._watchdogInterval);
    }
  }

  _setup() {
    this._connection.onreconnecting(err => {
      LogService.info("SignalR reconnecting: ", err);
      this._notifyConnectionStateChanged();
    });

    this._connection.onreconnected(connId => {
      LogService.info("SignalR reconnected: ", connId);
      this._notifyConnectionStateChanged();
    });

    this._connection.onclose(err => {
      LogService.error("SignalR errored", err);
      this._errorHandler(err);
      this._notifyConnectionStateChanged();
    });
  }

  _errorHandler = async error => {
    LogService.error("Error handler", error);

    if (!this._everBeenConnected) this.connectAsync();
  };
}

const singleton = new MessagingService();
export default singleton;
