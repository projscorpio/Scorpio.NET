import * as types from "./actionTypes";

export function setMapMarkers(markers) {
  return {
    type: types.SET_MAP_MARKERS,
    payload: markers
  };
}

export function setRoverPosition() {
  return {
    type: types.SET_ROVER_POSITION
  };
}
