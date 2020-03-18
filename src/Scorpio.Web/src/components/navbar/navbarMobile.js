import React from "react";
import { withRouter } from "react-router-dom";
import { Icon, Image, Menu, Sidebar } from "semantic-ui-react";
import MenuItems from "./menuItems";

const NavBarMobile = ({ history, children, onPusherClick, onToggle, visible }) => {
  const handleClick = (_, data) => {
    history.push(data.name);
  };

  return (
    <Sidebar.Pushable>
      <Sidebar as={Menu} animation="overlay" icon="labeled" inverted width="wide" vertical visible={visible}>
        <MenuItems onClick={handleClick} />
      </Sidebar>
      <Sidebar.Pusher dimmed={visible} onClick={onPusherClick} style={{ minHeight: "100vh" }}>
        <Menu fixed="top">
          <Menu.Item>
            <Image size="tiny" src={process.env.PUBLIC_URL + "/logo.png"} />
          </Menu.Item>
          <Menu.Item onClick={onToggle} position="right">
            <Icon name="sidebar" />
          </Menu.Item>
        </Menu>
        {children}
      </Sidebar.Pusher>
    </Sidebar.Pushable>
  );
};

export default withRouter(NavBarMobile);
