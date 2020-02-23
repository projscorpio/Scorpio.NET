import * as types from "./actionTypes";

export function setMapMarkers(markers) {
  return {
    type: types.SET_MAP_MARKERS,
    payload: markers
  };
}

export function fetchRoverPosition() {
  return {
    type: types.FETCH_ROVER_POSITION,
  };
}
