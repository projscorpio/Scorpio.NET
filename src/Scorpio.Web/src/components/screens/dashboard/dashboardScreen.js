import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import * as actions from "../../../actions";
import { WidthProvider, Responsive } from "react-grid-layout";
import { Responsive as SemanticResponsive, Icon, Dropdown, Menu, Segment, Checkbox } from "semantic-ui-react";
import WidgetErrorBoundary from "../../common/widgetErrorBoundary";
import { tryParseLayouts, guid } from "../../../utils/utils";
import OverlayContextMenu from "../../common/overlayContextMenu";
import "../../../../node_modules/react-grid-layout/css/styles.css";
import "../../../../node_modules/react-resizable/css/styles.css";
import { widgets } from "./widgets/widgets";
import DashboardWidgetWizard from "./dashboardWidgetWizard";
import WidgetErrorFallback from "../../common/widgetErrorFallback";
import ConfigurationModal from "./configurationModal";
import ConfigurationDropdown from "./configurationDropdown";
import LogService from "../../../services/LogService";

const ResponsiveReactGridLayout = WidthProvider(Responsive);

class DashboardScreen extends Component {
  constructor(props) {
    super(props);

    const lay = this.getRequestedLayout(props.state.configs);

    this.state = {
      allowRearrange: false,
      layouts: lay.layout,
      pageConfig: lay.config,
      runWizard: false,
      runNewConfigModal: false,
      editableConfig: null
    };

    this.widgetContainerRefs = [];
  }

  getRequestedLayout = configs => {
    const urlParams = new URLSearchParams(window.location.search);
    const queryParamPageName = urlParams.get("config");
    const decodedPage = decodeURIComponent(queryParamPageName);

    if (queryParamPageName && decodedPage) {
      if (Array.isArray(configs) && configs.length > 0) {
        const config = configs.find(x => x.name === decodedPage && x.type === "page") || {};
        //LogService.debug("Found matching page config", config);
        return {
          layout: tryParseLayouts(config.data),
          config
        };
      }
    } else {
      // there is no page query param - get default one (first one matching page type)
      if (Array.isArray(configs) && configs.length > 0) {
        const config = configs.filter(x => x.type === "page")[0] || {};
        //LogService.debug("Found default page config", config);
        return {
          layout: tryParseLayouts(config.data),
          config
        };
      }
    }

    return {};
  };

  resetLayout = () => {
    this.setState({ layouts: {} });
  };

  componentWillReceiveProps(nextProps) {
    if (nextProps.state.configs && nextProps.state.configs.length > 0) {
      const parsed = this.getRequestedLayout(nextProps.state.configs);
      this.setState({ layouts: parsed.layout, pageConfig: parsed.config });
    }
  }

  onLayoutChange = (layout, layouts) => {
    this.setState({ layouts });
  };

  handleAllowRearrangeChanged = (ev, data) => {
    if (data.checked === false) {
      const cfg = {
        ...this.state.pageConfig,
        data: JSON.stringify(this.state.layouts)
      };

      this.props.actions.setConfig(cfg);
    }
    this.setState({ allowRearrange: data.checked });
  };

  onAddWidgetClick = () => {
    this.setState({ runWizard: true });
  };

  onCloseWizard = () => {
    this.setState({ runWizard: false, editableConfig: null });
  };

  onWizardFinished = data => {
    LogService.debug("onWizardFinishied", data);
    const pageConfigId = this.state.pageConfig.id;
    const editingWidgetId = this.state.editableConfig && this.state.editableConfig.id;

    const cfg = {
      name: data.props.widgetTitle || guid(),
      type: "member",
      parentId: pageConfigId,
      id: editingWidgetId ? editingWidgetId : 0,
      data: data
    };

    this.props.actions.setConfig(cfg);
    this.onCloseWizard();
  };

  onWidgetEditClick = id => {
    const config = this.props.state.configs.find(x => x.id === id);
    this.setState({ runWizard: true, editableConfig: config });
  };

  onWidgetRemoveClick = id => {
    this.props.actions.deleteConfig(id);
  };

  onNewConfigurationClick = () => {
    this.setState({ runNewConfigModal: true });
  };

  onNewConfigurationClose = () => {
    this.setState({ runNewConfigModal: false });
  };

  onNewConfigurationAdd = async name => {
    this.onNewConfigurationClose();
    const cfg = {
      name,
      type: "page",
      parentId: 0,
      id: 0,
      data: {}
    };
    this.props.actions.setConfig(cfg);
  };

  removeConfigPage = async () => {
    if (window.confirm("Are you sure? You cannot undo this operation.")) {
      const pageConfigId = this.state.pageConfig.id;
      this.props.actions.deleteConfig(pageConfigId);
      this.props.history.push("/dashboard");
    }
  };

  changeScreen = screen => {
    if (!screen || !screen.name) return;
    const encodedName = encodeURIComponent(screen.name);
    const newUri = `/dashboard?config=${encodedName}`;
    this.props.history.push(newUri);
  };

  render() {
    const { configs } = this.props.state;
    const { allowRearrange, layouts, runWizard, editableConfig, runNewConfigModal, pageConfig } = this.state;
    const wizardInitialValues = editableConfig && editableConfig.data ? editableConfig.data : null;

    let currentPageMembers = [];
    if (pageConfig && pageConfig.id) {
      currentPageMembers = configs.filter(x => x.parentId === pageConfig.id && x.type === "member") || [];
    }

    return (
      <>
        {runWizard && (
          <DashboardWidgetWizard onClose={this.onCloseWizard} onSubmit={this.onWizardFinished} initialValues={wizardInitialValues} />
        )}
        {runNewConfigModal && <ConfigurationModal onClose={this.onNewConfigurationClose} onAdd={this.onNewConfigurationAdd} />}
        <Menu fluid borderless compact secondary>
          <Menu.Item>
            <Icon name="cubes" />
            <ConfigurationDropdown onSelect={this.changeScreen} onAddNew={this.onNewConfigurationClick} />
          </Menu.Item>
          <Menu.Item onClick={this.onAddWidgetClick}>
            <Icon name="add" />
            Add Widget
          </Menu.Item>
          <SemanticResponsive style={{ marginLeft: "auto" }} minWidth={SemanticResponsive.onlyMobile.maxWidth}>
            <Menu.Menu position="right">
              {allowRearrange && (
                <>
                  <Menu.Item onClick={this.resetLayout}>
                    <Icon name="undo" />
                    Reset layout
                  </Menu.Item>
                  <Menu.Item onClick={this.removeConfigPage}>
                    <Icon name="remove" />
                    Remove config
                  </Menu.Item>
                </>
              )}
              <Menu.Item>
                <Checkbox toggle fitted checked={allowRearrange} onChange={this.handleAllowRearrangeChanged} label="Modify" />
              </Menu.Item>
            </Menu.Menu>
          </SemanticResponsive>
          <SemanticResponsive style={{ marginLeft: "auto" }} maxWidth={SemanticResponsive.onlyMobile.maxWidth}>
            <Dropdown item icon="wrench" simple>
              <>
                Layout settings
                <Dropdown.Menu>
                  <Dropdown.Item>
                    <Checkbox toggle checked={allowRearrange} onChange={this.handleAllowRearrangeChanged} label="Modify" />
                  </Dropdown.Item>
                  <Dropdown.Item onClick={this.resetLayout}>Reset layout</Dropdown.Item>
                  <Dropdown.Item onClick={this.removeConfigPage}>Remove config</Dropdown.Item>
                </Dropdown.Menu>
              </>
            </Dropdown>
          </SemanticResponsive>
        </Menu>
        <Segment>
          {currentPageMembers.length === 0 ? (
            <div>No Widgets so far! Feel free to add new one.</div>
          ) : (
            <ResponsiveReactGridLayout
              cols={{ lg: 12, md: 10, sm: 6, xs: 4, xxs: 2 }}
              isRearrangeable={true}
              rowHeight={40}
              margin={[20, 20]}
              layouts={layouts}
              onLayoutChange={this.onLayoutChange}
              isResizable={allowRearrange}
              isDraggable={allowRearrange}
            >
              {currentPageMembers.map((obj, indx) => {
                if (!obj.data || !obj.data.type || !obj.data.props) return undefined;
                let DashboardWidget = widgets.find(x => x.type === obj.data.type);

                DashboardWidget !== undefined ? (DashboardWidget = DashboardWidget.component) : (DashboardWidget = WidgetErrorFallback);
                return (
                  <div key={indx} data-grid={{ w: 4, h: 3, x: 0, y: 0, minW: 2, minH: 2 }}>
                    <OverlayContextMenu
                      relatedId={obj.id}
                      showIcons={allowRearrange}
                      onEditClick={this.onWidgetEditClick}
                      onRemoveClick={this.onWidgetRemoveClick}
                      {...obj.data.props}
                    >
                      <WidgetErrorBoundary key={indx}>
                        <DashboardWidget containerRef={this.widgetContainerRefs[obj.type]} {...obj.data.props} key={indx} />
                      </WidgetErrorBoundary>
                    </OverlayContextMenu>
                  </div>
                );
              })}
            </ResponsiveReactGridLayout>
          )}
        </Segment>
      </>
    );
  }
}

function mapStateToProps(state) {
  return { state }; // TODO
}

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(DashboardScreen));
