import * as types from "../actions/actionTypes";
import initialState from "../store/initialState";

export default function canOpenReducer(state = initialState.canOpen, action) {
  switch (action.type) {
    case types.SET_CAN_OPEN_TREE:
      return doSetMiControlCanOpenTree(state, action);

    case types.SET_SCORPIO_CAN_OPEN_TREE:
      return doSetScorpioCanOpenTree(state, action);

    default:
      return state;
  }
}

function doSetMiControlCanOpenTree(state, action) {
  return Object.assign({}, state, { miControlCanTree: action.payload });
}

function doSetScorpioCanOpenTree(state, action) {
  return Object.assign({}, state, { scorpioCanTree: action.payload });
}
