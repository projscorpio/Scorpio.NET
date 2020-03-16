import React from "react";
import { Icon, Menu, Dropdown } from "semantic-ui-react";

const MenuItems = ({ onClick }) => {
  return (
    <>
      <Menu.Item name="/dashboard" as="a" onClick={onClick}>
        <Icon name="dashboard" />
        Dashboard
      </Menu.Item>
      <Menu.Item name="/control" as="a" onClick={onClick}>
        <Icon name="shipping fast" />
        Control
      </Menu.Item>
      {/* <Menu.Item name="/stream" as="a" onClick={onClick}>
        <Icon name="video" />
        Stream
      </Menu.Item>*/}
      <Menu.Item name="/map" as="a" onClick={onClick}>
        <Icon name="map" />
        Map
      </Menu.Item>
      {/* <Menu.Item name="/gamepad" as="a" onClick={onClick}>
        <Icon name="gamepad" />
        Gamepad
      </Menu.Item> */}
      <Menu.Item>
        <Icon name="chart line" />
        <Dropdown text="Science" pointing="top" lazyLoad>
          <Dropdown.Menu>
            <Dropdown.Item icon="database" label="Sensor data" name="/science/edit/sensor-data" as="a" onClick={onClick} />
            <Dropdown.Item icon="chart area" label="Charts" name="/science/sensor-charts" as="a" onClick={onClick} />
          </Dropdown.Menu>
        </Dropdown>
      </Menu.Item>
      <Menu.Item>
        <Icon name="wrench" />
        <Dropdown text="Advanced" pointing="top" lazyLoad>
          <Dropdown.Menu>
            <Dropdown.Item icon="microchip" label="Sensorics" name="/edit/sensor" as="a" onClick={onClick} />
            <Dropdown.Item icon="video" label="Streams" name="/edit/stream" as="a" onClick={onClick} />
            <Dropdown.Item icon="database" label="CAN Explorer" name="/can-open-explorer" as="a" onClick={onClick} />{" "}
            <Dropdown.Item icon="database" label="Scorpio CAN Explorer" name="/scorpio-can-open-explorer" as="a" onClick={onClick} />{" "}
            <Dropdown.Item icon="terminal" label="Console" name="/console" as="a" onClick={onClick} />
          </Dropdown.Menu>
        </Dropdown>
      </Menu.Item>
    </>
  );
};

export default MenuItems;
