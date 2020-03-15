import React, { Component } from "react";
import nipplejs from "nipplejs";
import { Checkbox, Segment } from "semantic-ui-react";
import { TOPICS } from "../../../constants/appConstants";
import MessagingService from "../../../services/MessagingService";

class Control extends Component {
  constructor(props) {
    super(props);
    this.lastVector = { x: 0, y: 0 };
    this.state = { x: 0, y: 0, enableMovement: false };
    this.interval = null;
    this.joystick = null;
  }

  componentDidMount() {
    this.joystick = nipplejs.create({
      zone: document.getElementById("scorpio-touch-joystick"),
      mode: "dynamic",
      color: "#e66b00",
      size: 200,
      multitouch: true
    });

    this.joystick.on("move", (_, data) => {
      this.lastVector = data.vector;
    });

    this.joystick.on("end", () => (this.lastVector = { x: 0, y: 0 }));

    this.interval = setInterval(() => {
      this.setState({ x: this.lastVector.x, y: this.lastVector.y });
      const { enableMovement } = this.state;
      const { scorpioMessaging } = window;

      if (scorpioMessaging && scorpioMessaging.isConnected() && enableMovement) {
        const roverControlCommand = this.buildRoverControlCommand(this.lastVector);
        const limitedRoverControlCommand = this.limitMaxSpeed(roverControlCommand, 0.2137);
        scorpioMessaging.send(TOPICS.ROVER_CONTROL, limitedRoverControlCommand);
      }
    }, 50);
  }

  componentWillUnmount() {
    if (this.interval) clearInterval(this.interval);
    if (this.joystick) this.joystick.destroy();
  }

  buildRoverControlCommand(vector) {
    if (!vector || !vector.x || !vector.y) return { acc: 0, dir: 0 };

    const selfRotateDelta = 0.2;
    // rotate in place
    if (Math.abs(vector.y) <= selfRotateDelta) {
      return {
        acc: Math.abs(vector.x),
        dir: vector.x > 0 ? 200 : -200
      };
    }

    return {
      acc: vector.y,
      dir: vector.x
    };
  }

  limitMaxSpeed(roverControlCommand, maxSpeedMultipler) {
    if (maxSpeedMultipler <= 0) throw new Error("Multiple cannot be <0");

    return {
      acc: roverControlCommand.acc * 800.0 * maxSpeedMultipler,
      dir: roverControlCommand.dir
    };
  }

  onToggle = (ev, data) => {
    ev.preventDefault();
    const { checked } = data;
    this.setState({ enableMovement: checked });

    const topic = checked ? TOPICS.ROVER_ARM : TOPICS.ROVER_DISARM;
    MessagingService.send(topic, { dummy: "" });
  };

  render() {
    const { x, y, enableMovement } = this.state;

    return (
      <div style={{ margin: "1em" }}>
        <Segment className="flex">
          <div className="inline">
            <Checkbox toggle checked={enableMovement} onChange={this.onToggle} label={<label>Enable movement</label>} />
          </div>
          <div className="inline" style={{ marginLeft: "auto", marginRight: 0 }}>
            X: {x.toFixed(2)} Y: {y.toFixed(2)}
          </div>
        </Segment>
        <div>
          <div id="scorpio-touch-joystick" style={{ border: "2px dashed gray", position: "relative", height: "70vh" }} />
        </div>
      </div>
    );
  }
}

export default Control;
