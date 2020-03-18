import React from "react";
import { withRouter } from "react-router-dom";
import { Image, Menu } from "semantic-ui-react";
import MenuItems from "./menuItems";

const NavBarDesktop = ({ history }) => {
  const handleClick = (_, data) => {
    history.push(data.name);
  };

  return (
    <Menu fixed="top">
      <Menu.Item>
        <Image className="pointer" size="tiny" src={process.env.PUBLIC_URL + "/logo.png"} onClick={_ => handleClick(_, { name: "/" })} />
      </Menu.Item>
      <MenuItems onClick={handleClick} />
    </Menu>
  );
};

export default withRouter(NavBarDesktop);
