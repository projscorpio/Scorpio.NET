import { combineReducers } from "redux";
import { connectRouter } from "connected-react-router";
import configReducer from "./configReducer";
import sensorReducer from "./sensorReducer";
import streamsReducer from "./streamReducer";
import canOpenReducer from "./canOpenReducer";
import mapReducer from "./mapReducer";

const rootReducer = history =>
  combineReducers({
    router: connectRouter(history),
    configs: configReducer,
    sensors: sensorReducer,
    streams: streamsReducer,
    canOpen: canOpenReducer,
    map: mapReducer
  });

export default rootReducer;
