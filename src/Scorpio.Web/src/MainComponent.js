import React, { Component } from "react";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { withRouter } from "react-router-dom";
import * as actions from "./actions";
import { Route, Switch, Redirect } from "react-router-dom";
import NotFound from "./components/common/notFound";
import NavBar from "./components/navbar/navbar";
import Alert from "react-s-alert";
import { genericApi } from "./api/genericApi";
import { API, TOPICS } from "./constants/appConstants";
import AlertDispatcher from "./services/AlertDispatcher";
import DashboardScreen from "./components/screens/dashboard/dashboardScreen";
import StreamScreen from "./components/screens/stream/streamScreen";
import GamepadScreen from "./components/screens/gamepad/gamepadScreen";
import AboutScreen from "./components/screens/about/aboutScreen";
import SensorsEditorScreen from "./components/screens/sensor-editor/sensorEditorScreen";
import StreamEditorScreen from "./components/screens/stream-editor/streamEditorScreen";
import ConsoleScreen from "./components/screens/console/consoleScreen";
import SensorDataEditorScreen from "./components/screens/sensorData-editor/sensorDataEditorScreen";
import ScienceChartsScreen from "./components/screens/scienceCharts/scienceChartsScreen";
import GamepadService from "./services/GamepadService";
import MessagingService from "./services/MessagingService";
import FiluRacer from "./components/common/filuRacer";
import Control from "./components/screens/control/control";
import CanOpenExplorer from "./components/screens/canOpenExplorer/canOpenExplorer";
import scorpioCanOpenExplorer from "./components/screens/canOpenExplorer/scorpioCanOpenExplorer";
import MapScreen from "./components/screens/map/mapScreen";
import LogService from "./services/LogService";
import RoverRenderer from "./components/screens/roverRenderer/roverRenderer";

class MainComponent extends Component {
  async componentDidMount() {
    this.initGamepad();
    await this.initMessagingAsync();

    const result = await genericApi(API.CONFIG.GET_ALL, "GET");
    if (result && result.response && result.response.ok && result.body && Array.isArray(result.body)) {
      const configs = result.body || [];
      this.props.actions.setConfigs(configs);
    } else {
      AlertDispatcher.dispatch({ type: "error", text: "Could not fetch configs - check if API is running" });
    }

    // TODO promise.all()
    const sensorRes = await genericApi(API.SENSORS.GET_ALL, "GET");
    this.props.actions.setSensors(sensorRes.body);
    const streamRes = await genericApi(API.STREAMS.GET_ALL, "GET");
    this.props.actions.setStreams(streamRes.body);
    const gpsMarkersRes = await genericApi(API.SENSOR_DATA.GET_ALL_FILTERED.format("gps-markers"), "GET");
    this.props.actions.setMapMarkers(gpsMarkersRes.body);

    await this.setInitialRoverPosition();
    await this.setInitialRoverAngle();

    MessagingService.subscribe(TOPICS.ROVER_GPS_POS, this.roverPosChangedHandler);
    MessagingService.subscribe(TOPICS.ROVER_COMPASS_ANGLE, this.roverAngleChangedHandler);
  }

  componentWillUnmount() {
    MessagingService.unsubscribe(TOPICS.ROVER_GPS_POS, this.roverPosChangedHandler);
    MessagingService.unsubscribe(TOPICS.ROVER_COMPASS_ANGLE, this.roverAngleChangedHandler);
  }

  setInitialRoverPosition = async () => {
    try {
      const lastRoverPos = await genericApi(API.SENSOR_DATA.GET_LATEST.format("gps"), "GET");
      this.props.actions.setRoverPosition(JSON.parse(lastRoverPos.body.value));
    } catch (err) {
      LogService.error("Cannot set initial rover position", err);
    }
  };

  setInitialRoverAngle = async () => {
    try {
      const lastRoverAngle = await genericApi(API.SENSOR_DATA.GET_LATEST.format("compass"), "GET");
      this.props.actions.setRoverAngle(JSON.parse(lastRoverAngle.body.value));
    } catch (err) {
      LogService.error("Cannot set initial rover angle", err);
    }
  };

  roverPosChangedHandler = data => {
    try {
      const roverPos = JSON.parse(data);
      LogService.info("Got new rover position", roverPos);
      this.props.actions.setRoverPosition(roverPos);
    } catch {}
  };

  roverAngleChangedHandler = data => {
    try {
      const roverAngle = JSON.parse(data);
      LogService.info("Got new rover angle", roverAngle);
      this.props.actions.setRoverAngle(roverAngle);
    } catch {}
  };

  async initMessagingAsync() {
    window.scorpioMessaging = MessagingService;
    await MessagingService.connectAsync();
  }

  initGamepad() {
    window.scorpioGamepad = GamepadService;
    GamepadService.init();
  }

  render() {
    return (
      <>
        <NavBar className="fullWidth fullHeight">
          <Switch>
            <Route exact path="/" render={_ => <Redirect to={"/dashboard"} />} />
            <Route exact path="/dashboard" component={DashboardScreen} />
            <Route exact path="/stream" component={StreamScreen} />
            <Route exact path="/gamepad" component={GamepadScreen} />
            <Route exact path="/map" component={MapScreen} />
            <Route exact path="/control" component={Control} />
            <Route exact path="/about" component={AboutScreen} />
            <Route exact path="/rover-renderer" component={RoverRenderer} />
            <Route exact path="/edit/sensor" component={SensorsEditorScreen} />
            <Route exact path="/edit/stream" component={StreamEditorScreen} />
            <Route exact path="/science/edit/sensor-data/:sensorKey?/:id?" component={SensorDataEditorScreen} />
            <Route exact path="/science/sensor-charts" component={ScienceChartsScreen} />
            <Route exact path="/console" component={ConsoleScreen} />
            <Route exact path="/can-open-explorer" component={CanOpenExplorer} />
            <Route exact path="/scorpio-can-open-explorer" component={scorpioCanOpenExplorer} />
            <Route exact path="/filu" component={FiluRacer} />
            <Route exact path="/not-found" component={NotFound} />
            <Redirect to="/not-found" />
          </Switch>
        </NavBar>
        <Alert stack={{ limit: 2 }} beep timeout={5000} />
      </>
    );
  }
}

function mapStateToProps(state) {
  return { state };
}

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(MainComponent));
