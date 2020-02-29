import * as types from "../actions/actionTypes";
import initialState from "../store/initialState";

export default function canOpenReducer(state = initialState.canOpen, action) {
  switch (action.type) {
    case types.SET_CAN_OPEN_TREE:
      return doSetCanOpenTree(state, action);

    default:
      return state;
  }
}

function doSetCanOpenTree(state, action) {
  return Object.assign({}, state, { tree: action.payload });
}
