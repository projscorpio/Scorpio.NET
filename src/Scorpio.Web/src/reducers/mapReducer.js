import * as types from "../actions/actionTypes";
import initialState from "../store/initialState";

export default function mapReducer(state = initialState.map, action) {
  switch (action.type) {
    case types.SET_MAP_MARKERS:
      return doSetMarkers(state, action);

    case types.ADD_MAP_MARKER:
      return doSetMapMarker(state, action);

    case types.SET_ROVER_POSITION:
      return { ...state, roverPosition: action.payload };

    case types.SET_ROVER_ANGLE:
      return { ...state, roverRotation: action.payload };

    default:
      return state;
  }
}

function doSetMarkers(state, action) {
  return { ...state, markers: extractMarkers(action.payload) };
}

// payload is raw http response, therefore some mapping is required
function extractMarkers(payload) {
  return payload
    ? payload.map(x => {
        return {
          id: x.id,
          name: x.comment,
          timeStamp: x.timeStamp,
          ...JSON.parse(x.value)
        };
      })
    : [];
}

function doSetMapMarker(state, action) {
  const currentMarkers = state.markers;
  return { ...state, markers: [...currentMarkers, action.payload] };
}
