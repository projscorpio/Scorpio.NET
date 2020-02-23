import * as types from "../actions/actionTypes";
import initialState from "../store/initialState";

export default function mapReducer(state = initialState.map, action) {
  switch (action.type) {
    case types.SET_MAP_MARKERS:
      return { ...state, markers: action.payload };
    case types.FETCH_ROVER_POSITION:
      return { ...state };
    default:
      return state;
  }
}
