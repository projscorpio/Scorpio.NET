import React, { Component } from "react";
import { Responsive } from "semantic-ui-react";
import NavBarDesktop from "./navbarDesktop";
import NavBarMobile from "./navbarMobile";

export default class NavBar extends Component {
  state = {
    visible: false
  };

  handlePusher = () => {
    const { visible } = this.state;
    if (visible) this.setState({ visible: false });
  };

  handleToggle = () => this.setState({ visible: !this.state.visible });

  render() {
    const { children } = this.props;
    const { visible } = this.state;

    return (
      <>
        <Responsive {...Responsive.onlyMobile}>
          <NavBarMobile onPusherClick={this.handlePusher} onToggle={this.handleToggle} visible={visible}>
            <NavBarChildren>{children}</NavBarChildren>
          </NavBarMobile>
        </Responsive>
        <Responsive minWidth={Responsive.onlyTablet.minWidth}>
          <NavBarDesktop />
          <NavBarChildren>{children}</NavBarChildren>
        </Responsive>
      </>
    );
  }
}

const NavBarChildren = ({ children }) => <div className="scorpio-page-wrapper">{children}</div>;
