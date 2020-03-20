import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { Button } from "semantic-ui-react";
import { Map, TileLayer } from "react-leaflet";
import { genericApi } from "../../../api/genericApi";
import { API } from "../../../constants/appConstants";
import * as actions from "../../../actions";
import AlertDispatcher from "../../../services/AlertDispatcher";
import PositionMarkers from "./positionMarkers";
import RoverMarker from "./roverMarker";
import AddMarkerWizard from "./addMarkerWizard";
import ContextMenu from "./contextMenu";

import "leaflet/dist/leaflet.css";
import "./leafletCssWebpakWorkaround";

const DEFAULT_VIEWPORT = {
  center: [51.105, 17.038538], // somewhere in Wroclaw
  zoom: 13
};

class MapWidget extends Component {
  constructor(props) {
    super(props);
    this.state = {
      viewport: null,
      showWizard: false,
      editableMarker: null,
      contextMenuCoords: { x: 0, y: 0 },
      isContextOpened: false,
      useOnlineMap: true
    };
    this.contextMenuRef = React.createRef();
    this.currentZoom = DEFAULT_VIEWPORT.zoom;
  }

  onEditClicked = marker => {
    this.setState({ showWizard: true, editableMarker: marker });
  };

  handleAddMarker = async data => {
    const { editableMarker } = this.state;
    const payload = Object.assign({}, data); // make copy, so we can delete name property without invaldiating form
    const markerName = payload.name;
    const timeStamp = payload.timeStamp;
    delete payload.name;
    delete payload.id;
    delete payload.timeStamp;

    const sensorData = {
      id: editableMarker ? editableMarker.id : undefined,
      timeStamp: timeStamp,
      sensorKey: "gps-markers",
      value: JSON.stringify(payload),
      comment: markerName
    };

    const method = editableMarker ? "PUT" : "POST";
    const url = editableMarker ? API.SENSOR_DATA.UPDATE.format(editableMarker.id) : API.SENSOR_DATA.ADD;
    const resp = await genericApi(url, method, sensorData);
    if (resp.response.ok) {
      this.setState({ showWizard: false, editableMarker: null });
      const { data } = resp.body;

      const marker = { ...payload, name: data.comment, timeStamp: data.timeStamp, id: data.id };
      if (editableMarker) this.props.actions.updateMapMarker(marker);
      else this.props.actions.addMapMarker(marker); // save in redux store
    }
  };

  onRemoveClicked = async (ev, marker) => {
    ev.preventDefault();
    if (window.confirm("Are you sure?")) {
      await genericApi(API.SENSOR_DATA.DELETE.format(marker.id), "DELETE");
      this.props.actions.removeMapMarker(marker.id);
    }
  };

  onCenterMapClick = ev => {
    ev.preventDefault();
    const { roverPosition } = this.props.state.map;
    if (!roverPosition.latitude) {
      AlertDispatcher.disdispatchErrorpatch("No rover pos!");
      return;
    }

    this.setState({
      viewport: {
        zoom: this.currentZoom,
        center: [roverPosition.latitude, roverPosition.longitude]
      }
    });
  };

  onRightMapClick = ev => {
    this.contextMenuLatlng = ev.latlng;
    this.setState({
      ...this.state,
      isContextOpened: true,
      contextMenuCoords: {
        ...ev.containerPoint
      }
    });
  };

  onContextAddMarkerClick = async ev => {
    ev.preventDefault();

    this.setState({ isContextOpened: true });

    await this.handleAddMarker({
      name: "",
      latitude: this.contextMenuLatlng.lat,
      longitude: this.contextMenuLatlng.lng
    });
  };

  render() {
    const { showWizard, editableMarker, contextMenuCoords, isContextOpened, useOnlineMap, viewport } = this.state;

    return (
      <>
        {showWizard && (
          <AddMarkerWizard
            initialValues={editableMarker}
            onClose={() => this.setState({ showWizard: false })}
            onSubmit={this.handleAddMarker}
          />
        )}
        <ContextMenu
          mountRef={this.contextMenuRef}
          isOpened={isContextOpened}
          useOnlineMap={useOnlineMap}
          onAddMarkerClick={this.onContextAddMarkerClick}
          onOnlineChanged={isOnline => this.setState({ useOnlineMap: isOnline })}
        />
        <Map
          style={{ width: "100%", height: "100%", zIndex: 9 }}
          onclick={_ => this.setState({ isContextOpened: false })}
          viewport={viewport || DEFAULT_VIEWPORT}
          oncontextmenu={this.onRightMapClick}
          onzoomend={ev => (this.currentZoom = ev.target._zoom)}
        >
          <Button
            icon="crosshairs"
            color="blue"
            className="leaflet-top leaflet-left enable-events"
            style={{ marginTop: "70px", marginLeft: "5px", zIndex: 401 }}
            onClick={this.onCenterMapClick}
          />
          <Button
            className="leaflet-bottom leaflet-left enable-events"
            style={{ marginBottom: "20px", marginLeft: "5px", zIndex: 401 }}
            primary
            onClick={ev => this.setState({ showWizard: true })}
          >
            Add Marker
          </Button>
          <TileLayer url={useOnlineMap ? "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" : "/map/map_images/{z}/{x}/{y}.png"} />
          <PositionMarkers onEditClicked={this.onEditClicked} onRemoveClicked={this.onRemoveClicked} />
          <RoverMarker rotate />
          <div
            ref={this.contextMenuRef}
            style={{
              position: "relative",
              left: `${contextMenuCoords.x}px`,
              top: `${contextMenuCoords.y}px`
            }}
          />
        </Map>
      </>
    );
  }
}

function mapStateToProps(state) {
  return { state };
}

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(MapWidget));
