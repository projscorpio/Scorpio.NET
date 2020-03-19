import React, { useState } from "react";
import { Popup, Menu, Checkbox, Icon } from "semantic-ui-react";

const ContextMenu = ({ mountRef, isOpened, useOnlineMap, onAddMarkerClick, onOnlineChanged }) => {
  const [isOnline, setIsOnline] = useState(useOnlineMap);

  const handleToggle = (ev, { checked }) => {
    ev.preventDefault();
    setIsOnline(checked);
    if (typeof onOnlineChanged === "function") onOnlineChanged(checked);
  };

  return (
    <Popup context={mountRef} open={isOpened}>
      <Menu secondary vertical>
        <Menu.Item onClick={onAddMarkerClick}>
          <Icon name="marker" />
          Add marker
        </Menu.Item>
        <Menu.Item>
          <Icon name="wifi" />
          <Checkbox toggle checked={isOnline} onChange={handleToggle} label={<label>{isOnline ? "Go Offline" : "Go Online"}</label>} />
        </Menu.Item>
      </Menu>
    </Popup>
  );
};

export default ContextMenu;
