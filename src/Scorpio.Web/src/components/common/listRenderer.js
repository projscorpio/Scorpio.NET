import React from "react";

export const ListRenderer = ({ list, idKey, textKey }) => {
  if (!list && list.length === 0) return null;
  return (
    <ul style={{ padding: 0, listStyle: "none" }}>
      {list.map(item => {
        return <li key={item[idKey]}>{item[textKey]}</li>;
      })}
    </ul>
  );
};

export const ObjectAsListRenderer = ({ object, keysToSkip, dictionaryResolvers, keyTransforms }) => {
  if (!object) return null;

  const shouldSkip = key => keysToSkip && keysToSkip.includes(key);
  const keyKeyFinalForm = key => {
    if (keyTransforms && keyTransforms.includes("upper")) return capitalize(key);
    return key;
  };

  const renderSingle = (key, object) => {
    const hasDictionaryResolver = key => !!dictionaryResolvers[key];
    const getResolver = key => dictionaryResolvers[key];

    if (hasDictionaryResolver) {
      const resolver = getResolver(key);
      if (typeof resolver === "function") {
        const value = resolver(key, object[key]);
        return (
          <span>
            {keyKeyFinalForm(key)} - {value}
          </span>
        );
      }
    }

    return (
      <span>
        {keyKeyFinalForm(key)} - {object[key]}
      </span>
    );
  };

  return (
    <ul style={{ padding: 0, listStyle: "none" }}>
      {Object.keys(object).map(key => {
        if (shouldSkip(key)) return null;
        return <li key={key}>{renderSingle(key, object)}</li>;
      })}
    </ul>
  );
};

const capitalize = s => {
  if (typeof s !== "string") return "";
  return s.charAt(0).toUpperCase() + s.slice(1);
};
