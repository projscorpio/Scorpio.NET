import update from "immutability-helper";
import * as types from "../actions/actionTypes";
import initialState from "../store/initialState";
import AlertDispatcher from "../services/AlertDispatcher";
import LogService from "../services/LogService";
import { genericApi } from "../api/genericApi";
import { API } from "../constants/appConstants";

export default function configReducer(state = initialState.configs, action) {
  switch (action.type) {
    case types.SET_CONFIGS:
      return doSetConfigs(state, action);

    case types.DELETE_CONFIG:
      return doDeleteConfig(state, action);

    case types.SET_CONFIG:
      return doSetConfig(state, action);

    case types.REPLACE_CONFIG_ID:
      return doReplaceGuidByDbId(state, action);

    default:
      return state;
  }
}

function doSetConfigs(state, action) {
  try {
    return jsonParseDataProperty(action.payload);
  } catch (err) {
    const msg = "CRITICAL: Could not deserialize config from database (not a valid JSON)";
    AlertDispatcher.dispatchError(msg);
    LogService.error(msg);
    LogService.error(err);

    // self healing
    removeInvalidConfigs(action.payload);
    return state;
  }
}

async function removeInvalidConfigs(configs) {
  if (configs && Array.isArray(configs)) {
    for (const config of configs) {
      if (config.id) {
        LogService.info("Trying to self heal by removing invalid config: ", config);
        await genericApi(API.CONFIG.DELETE_BY_ID.format(config.id), "DELETE");
      }
    }
  }
}

// deserialize json (stored in DB as json) and flatten with addding id property
const jsonParseDataProperty = arr => {
  return !arr || !Array.isArray(arr)
    ? []
    : arr.map(x => {
        return {
          ...x,
          data: JSON.parse(x.data)
        };
      });
};

function doDeleteConfig(state, action) {
  return state.filter(x => x.id !== action.payload);
}

function doSetConfig(state, action) {
  const configId = action.payload.config.id;

  // If its add, then we have GUID, else we got databse ID (string without dash)
  const isUpdate = configId && typeof configId === "string" && configId.indexOf("-") === -1;

  if (isUpdate) {
    // update array item
    let configsCopy = Object.assign([], state); // create a copy, so we dont mutate state
    const toPurge = configsCopy.find(x => x.id === configId); // find existing config
    const index = configsCopy.indexOf(toPurge);
    if (index !== -1) configsCopy.splice(index, 1); // remove old config from configs array
    configsCopy.splice(index, 0, action.payload.config); // insert update element at index of removed one

    return configsCopy;
  } else {
    // Adding new one
    return update(state, {
      $push: [action.payload.config]
    });
  }
}

function doReplaceGuidByDbId(state, action) {
  const { oldGuid, newId } = action.payload;

  return state.map(item => {
    if (item.id !== oldGuid) return item;
    return {
      ...item,
      id: newId
    };
  });
}
