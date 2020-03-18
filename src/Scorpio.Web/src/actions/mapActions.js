import * as types from "./actionTypes";

export function setMapMarkers(markers) {
  return {
    type: types.SET_MAP_MARKERS,
    payload: markers
  };
}

export function addMapMarker(marker) {
  return {
    type: types.ADD_MAP_MARKER,
    payload: marker
  };
}

export function updateMapMarker(marker) {
  return {
    type: types.UPDATE_MAP_MARKER,
    payload: marker
  };
}

export function removeMapMarker(markerId) {
  return {
    type: types.REMOVE_MAP_MARKER,
    payload: markerId
  };
}

export function setRoverPosition(pos) {
  return {
    type: types.SET_ROVER_POSITION,
    payload: pos
  };
}

export function setRoverAngle(angle) {
  return {
    type: types.SET_ROVER_ANGLE,
    payload: angle
  };
}
