import { combineReducers } from "redux";
import { connectRouter } from "connected-react-router";
import configReducer from "./configReducer";
import sensorReducer from "./sensorReducer";
import streamsReducer from "./streamReducer";
import mapReducer from "./mapReducer";

const rootReducer = history =>
  combineReducers({
    router: connectRouter(history),
    configs: configReducer,
    sensors: sensorReducer,
    streams: streamsReducer,
    map: mapReducer
  });

export default rootReducer;
