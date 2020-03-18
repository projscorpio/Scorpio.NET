import React, { Component } from "react";
import { connect, useSelector } from "react-redux";
import { bindActionCreators } from "redux";
import { withRouter } from "react-router-dom";
import { Segment, Message, Table, TableCell, Button, Menu, Dropdown, Icon, Checkbox } from "semantic-ui-react";
import * as actions from "../../../actions";
import { genericApi } from "../../../api/genericApi";
import { API } from "../../../constants/appConstants";
import AddButtonMenuContainer from "../../common/addButtonMenuContainer";
import Spinner from "../../common/spinner";
import Pager from "../../common/pager";
import SensorDataWizard from "./sensorDataEditorWizard";
import WipeDataWizard from "./wipeDataWizard";

const FILTER_ALL_KEY = "__all";

class SensorDataEditorScreen extends Component {
  constructor(props) {
    super(props);

    // Retrieve sensorKey from url if present
    const { sensorKey } = props.match.params;

    this.state = {
      entities: [],
      runWizard: false,
      runWipeWizard: false,
      isFetched: false,
      currentPage: 1,
      itemsPerPage: 50,
      editingEntity: null,
      selectedSensor: sensorKey ? sensorKey : FILTER_ALL_KEY,
      showId: false
    };
  }

  async componentDidMount() {
    const { currentPage, itemsPerPage, selectedSensor } = this.state;

    const idFromUrl = this.props.match.params.id;
    const areWeEditing = idFromUrl !== undefined;

    if (areWeEditing) {
      await this.fetchEditingItem(idFromUrl);
    } else {
      await this.fetchItems(currentPage, itemsPerPage, selectedSensor);
    }
  }

  // runs only if there is ID specified in URL - we need this specific item, as it might not be at current page
  fetchEditingItem = async id => {
    const result = await genericApi(API.SENSOR_DATA.GET_BY_ID.format(id), "GET");
    if (result.response.ok) {
      this.setState({ editingEntity: result.body, runWizard: true, isFetched: true });
    }
  };

  fetchItems = async (currentPage, itemsPerPage, sensorKey) => {
    this.setState({ isFetched: false });
    let endpoint = API.SENSOR_DATA.GET_PAGED.format(currentPage, itemsPerPage);

    // filtering is selected - get by selected sensorKey
    if (sensorKey && sensorKey !== FILTER_ALL_KEY) {
      endpoint = API.SENSOR_DATA.GET_PAGED_FILTERED.format(sensorKey, currentPage, itemsPerPage);
    }

    const result = await genericApi(endpoint, "GET");
    if (result.response.ok) {
      this.setState({ entities: result.body.values, isFetched: true });
    }
  };

  onItemsPerPageChanged = async itemsPerPage => {
    const { currentPage, selectedSensor } = this.state;
    this.setState({ isFetched: false, itemsPerPage: itemsPerPage });
    await this.fetchItems(currentPage, itemsPerPage, selectedSensor);
  };

  onPageChange = async pageNum => {
    const { itemsPerPage, selectedSensor } = this.state;
    this.setState({ isFetched: false, currentPage: pageNum });
    await this.fetchItems(pageNum, itemsPerPage, selectedSensor);
  };

  handleRemoveClick = async entity => {
    if (window.confirm(`Are you sure you want to remove sensor data ${entity.name}?`)) {
      const { currentPage, itemsPerPage, selectedSensor } = this.state;
      await genericApi(API.SENSOR_DATA.DELETE.format(entity.id), "DELETE");
      await this.fetchItems(currentPage, itemsPerPage, selectedSensor);
      this.setState({ runWizard: false });
    }
  };

  handleEditClick = entity => {
    this.setState({ runWizard: true, editingEntity: entity });
  };

  handleAddClick = () => {
    let editingEntity = null;
    const { selectedSensor } = this.state;

    // if filter by some sensor is selected, use this sensor in add wizard
    if (selectedSensor !== FILTER_ALL_KEY) {
      editingEntity = { sensorKey: selectedSensor };
    }

    this.setState({ editingEntity: editingEntity, runWizard: true });
  };

  onWizardFinished = async data => {
    const { editingEntity, currentPage, itemsPerPage, selectedSensor } = this.state;
    const isUpdate = editingEntity !== null;
    const url = isUpdate ? API.SENSOR_DATA.UPDATE.format(data.id) : API.SENSOR_DATA.ADD;
    await genericApi(url, isUpdate ? "PUT" : "POST", data);
    await this.fetchItems(currentPage, itemsPerPage, selectedSensor);
    this.setState({ editingEntity: null, runWizard: false });
    this.fetchItems(currentPage, itemsPerPage, selectedSensor);
  };

  onCloseWizard = () => this.setState({ runWizard: false });

  onFilterChange = async sensorKey => {
    const { itemsPerPage, currentPage } = this.state;
    this.setState({ selectedSensor: sensorKey });
    this.fetchItems(currentPage, itemsPerPage, sensorKey);
  };

  onWipeDataClick = () => {
    this.setState({ runWipeWizard: true });
  };

  onWipeWizardSubmit = async data => {
    const { selectedSensor, currentPage, itemsPerPage } = this.state;
    this.setState({ runWipeWizard: false });

    let url = new URL(API.SENSOR_DATA.DELETE_MANY.format(data.sensorKey));

    if (data.from) url.searchParams.append("from", data.from);
    if (data.to) url.searchParams.append("to", data.to);

    await genericApi(url.toString(), "DELETE");
    this.fetchItems(currentPage, itemsPerPage, selectedSensor);
  };

  renderValue = entity => {
    try {
      if (entity.sensorKey.startsWith("gps")) return this.renderGpsValue(entity.value);
      else return entity.value;
    } catch {
      return "Invalid value for given sensor type";
    }
  };

  renderGpsValue = value => {
    const pos = JSON.parse(value);
    return (
      <span>
        Lat: {pos.latitude} Lon: {pos.longitude}
      </span>
    );
  };

  render() {
    const { isFetched, entities, runWizard, runWipeWizard, itemsPerPage, currentPage, editingEntity, selectedSensor, showId } = this.state;
    const hasData = Array.isArray(entities) && entities.length > 0;
    const renderSensorKeyCol = selectedSensor && selectedSensor === FILTER_ALL_KEY;
    const renderIdCol = !!showId;

    return (
      <>
        {runWizard && <SensorDataWizard initialValues={editingEntity} onClose={this.onCloseWizard} onSubmit={this.onWizardFinished} />}
        {runWipeWizard && <WipeDataWizard onClose={_ => this.setState({ runWipeWizard: false })} onSubmit={this.onWipeWizardSubmit} />}
        <Segment attached="bottom" style={{ padding: "1em" }}>
          <AddButtonMenuContainer
            addText={"Add new data point"}
            onAddClick={() => this.handleAddClick()}
            customLeftItem={
              <AdditionalMenuItems
                onChange={this.onFilterChange}
                showId={showId}
                onShowIdChanged={checked => this.setState({ showId: checked })}
                defaultFilter={selectedSensor}
                onWipeDataClick={this.onWipeDataClick}
              />
            }
          >
            {isFetched ? (
              <>
                {hasData ? (
                  <Table selectable celled color="orange">
                    <Table.Header>
                      <Table.Row>
                        {renderIdCol && <Table.HeaderCell width="2">Id</Table.HeaderCell>}
                        {renderSensorKeyCol && <Table.HeaderCell>Sensor Key</Table.HeaderCell>}
                        <Table.HeaderCell>Time</Table.HeaderCell>
                        <Table.HeaderCell>Value</Table.HeaderCell>
                        <Table.HeaderCell>Comment</Table.HeaderCell>
                        <Table.HeaderCell width="2">Actions</Table.HeaderCell>
                      </Table.Row>
                    </Table.Header>
                    <Table.Body>
                      {entities.map(x => {
                        return (
                          <Table.Row key={x.id}>
                            {renderIdCol && <TableCell>{x.id}</TableCell>}
                            {renderSensorKeyCol && <TableCell>{x.sensorKey}</TableCell>}
                            <TableCell>{x.timeStamp ? new Date(x.timeStamp).toLocaleString() : ""}</TableCell>
                            <TableCell>{this.renderValue(x)}</TableCell>
                            <TableCell>{x.comment}</TableCell>
                            <TableCell>
                              <Button icon="edit" color="grey" onClick={() => this.handleEditClick(x)} />
                              <Button icon="remove" color="red" onClick={() => this.handleRemoveClick(x)} />
                            </TableCell>
                          </Table.Row>
                        );
                      })}
                    </Table.Body>
                  </Table>
                ) : (
                  <Message color="yellow" header="There is no entities yet" list={["You must add new entities to see the list here."]} />
                )}
              </>
            ) : (
              <Spinner />
            )}
            <Pager
              currentPage={currentPage}
              itemsPerPage={itemsPerPage}
              onItemsPerPageChanged={this.onItemsPerPageChanged}
              onPageChange={this.onPageChange}
            />
          </AddButtonMenuContainer>
        </Segment>
      </>
    );
  }
}

const AdditionalMenuItems = ({ defaultFilter, onChange, showId, onShowIdChanged, onWipeDataClick }) => {
  const sensors = useSelector(x => x.sensors);
  let options = Array.isArray(sensors)
    ? sensors.map(sensor => {
        return {
          key: sensor.sensorKey,
          value: sensor.sensorKey,
          text: sensor.name
        };
      })
    : [];

  options.unshift({ key: FILTER_ALL_KEY, value: FILTER_ALL_KEY, text: "All" });

  const handleChange = (ev, d) => {
    if (typeof onChange === "function") {
      onChange(d.value);
    }
  };

  const handleShowIdChange = (ev, d) => {
    if (typeof onShowIdChanged === "function") {
      onShowIdChanged(d.checked);
    }
  };

  const handleWipeDataClick = (ev, d) => {
    if (typeof onWipeDataClick === "function") {
      onWipeDataClick(ev, d);
    }
  };

  return (
    <>
      <Menu.Item style={{ paddingRight: "1rem" }}>
        <Checkbox label="Show ID" style={{ paddingLeft: "1rem" }} onChange={handleShowIdChange} checked={!!showId} />
      </Menu.Item>
      <Menu.Item>
        <Icon name="filter" />
        Select sensor
        <Dropdown style={{ paddingLeft: "1rem" }} options={options} defaultValue={defaultFilter} onChange={handleChange} />
      </Menu.Item>
      <Menu.Item position="right" onClick={handleWipeDataClick}>
        <Icon name="eraser" color="red" />
        Wipe data
      </Menu.Item>
    </>
  );
};

function mapStateToProps(state) {
  return { state };
}

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(SensorDataEditorScreen));
