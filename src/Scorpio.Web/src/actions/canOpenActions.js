import * as types from "../actions/actionTypes";

export function setCanOpenTree(tree) {
  return {
    type: types.SET_CAN_OPEN_TREE,
    payload: tree
  };
}

export function setScorpioCanOpenTree(tree) {
  return {
    type: types.SET_SCORPIO_CAN_OPEN_TREE,
    payload: tree
  };
}
