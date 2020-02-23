import React, { PureComponent } from "react";
import PropTypes from "prop-types";
import { Menu } from "semantic-ui-react";

class OverlayContextMenu extends PureComponent {
  onEditClick = (ev, data) => {
    if (this.props.onEditClick) this.props.onEditClick(this.props.relatedId);
  };

  onRemoveClick = (ev, data) => {
    if (this.props.onRemoveClick) this.props.onRemoveClick(this.props.relatedId);
  };

  render() {
    const { showIcons, widgetTitle } = this.props;

    if (showIcons) {
      return (
        <>
          <Menu secondary borderless icon style={{ zIndex: "999", display: "flex", justifyContent: "flex-end" }}>
            <Menu.Item icon="edit outline" onClick={this.onEditClick} />
            <Menu.Item icon="close" onClick={this.onRemoveClick} />
          </Menu>
          <div
            className="fullHeight fullWidth center"
            style={{ display: "table", opacity: "0.25", position: "relative", top: "-56px", zIndex: "-1" }}
          >
            <div style={{ display: "table-cell", verticalAlign: "middle" }}>{this.props.children}</div>
          </div>
        </>
      );
    }

    return (
      <div className="fullWidth fullHeight bordered" style={{ display: "table" }}>
        <div className="fullWidth center">
          <h4 className="padding-bottom-sm">{widgetTitle}</h4>
        </div>
        <div className="center fullWidth fullHeight" style={{ display: "table-row" }}>
          <div className="fullWidth fullHeight"
               style={{ display: "table-cell", verticalAlign: "middle", }}>{this.props.children}
          </div>
        </div>
      </div>
    );
  }
}

export default OverlayContextMenu;

OverlayContextMenu.propTypes = {
  relatedId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
  showIcons: PropTypes.bool.isRequired,
  onEditClick: PropTypes.func.isRequired,
  onRemoveClick: PropTypes.func.isRequired
};
