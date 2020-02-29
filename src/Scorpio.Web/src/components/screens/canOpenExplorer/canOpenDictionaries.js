export const dataTypeDict = Object.freeze({
  UNKNOWN: "0x00",
  BOOLEAN: "0x01",
  INTEGER8: "0x02",
  INTEGER16: "0x03",
  INTEGER32: "0x04",
  UNSIGNED8: "0x05",
  UNSIGNED16: "0x06",
  UNSIGNED32: "0x07",
  REAL32: "0x08",
  VISIBLE_STRING: "0x09",
  OCTET_STRING: "0x0a",
  UNICODE_STRING: "0x0b",
  TIME_OF_DAY: "0x0c",
  TIME_DIFFERENCE: "0x0d",
  DOMAIN: "0x0f",
  INTEGER24: "0x10",
  REAL64: "0x11",
  INTEGER40: "0x12",
  INTEGER48: "0x13",
  INTEGER56: "0x14",
  INTEGER64: "0x15",
  UNSIGNED24: "0x16",
  UNSIGNED40: "0x18",
  UNSIGNED48: "0x19",
  UNSIGNED56: "0x1a",
  UNSIGNED64: "0x1b",

  PDO_COMMUNICATION_PARAMETER: "0x20",
  PDO_MAPPING: "0x21",
  SDO_PARAMETER: "0x22",
  IDENTITY: "0x23"
});

export const getDataTypeDropdownOptions = () => {
  return Object.keys(dataTypeDict).map(key => {
    return {
      text: key,
      value: dataTypeDict[key]
    };
  });
};

export const getKeyByValue = (object, value) => {
  return Object.keys(object).find(key => object[key] === value);
};
