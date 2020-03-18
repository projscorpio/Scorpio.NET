import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { Button } from "semantic-ui-react";
import { Map, TileLayer } from "react-leaflet";
import PositionMarkers from "./positionMarkers";
import RoverMarker from "./roverMarker";
import * as actions from "../../../actions";
import AddMarkerWizard from "./addMarkerWizard";
import { genericApi } from "../../../api/genericApi";
import { API } from "../../../constants/appConstants";

import "leaflet/dist/leaflet.css";
import "./leafletCssWebpakWorkaround";

class MapWidget extends Component {
  constructor(props) {
    super(props);
    this.state = { showWizard: false, editableMarker: null };
    this.mapRef = React.createRef();
  }

  onEditClicked = marker => {
    this.setState({ showWizard: true, editableMarker: marker });
  };

  handleAddMarker = async data => {
    const { editableMarker } = this.state;
    const payload = Object.assign({}, data); // make copy, so we can delete name property without invaldiating form
    const markerName = payload.name;
    delete payload.name;
    delete payload.id;
    delete payload.timeStamp;

    const sensorData = {
      id: editableMarker ? editableMarker.id : undefined,
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

  render() {
    const { showWizard, editableMarker } = this.state;
    const { roverPosition } = this.props.state.map;

    console.log(this.mapRef.current);
    // TODO: follow rover switch or 'center' button to follow rover
    const center = roverPosition.latitude ? [roverPosition.latitude, roverPosition.longitude] : [51.107883, 17.038538]; //- wro

    return (
      <>
        {showWizard && (
          <AddMarkerWizard
            initialValues={editableMarker}
            onClose={() => this.setState({ showWizard: false })}
            onSubmit={this.handleAddMarker}
          />
        )}
        <Map
          style={{
            width: "100%",
            height: "100%",
            zIndex: 1
          }}
          attributionControl={true}
          zoomControl={true}
          ref={this.mapRef}
          onclick={ev => console.log(ev)}
          center={center}
          zoom={13}
          duration={2000}
        >
          <TileLayer url="/map/map_images/{z}/{x}/{y}.png" />
          <PositionMarkers onEditClicked={this.onEditClicked} onRemoveClicked={this.onRemoveClicked} />
          <RoverMarker rotate />
        </Map>
        <Button className="scorpio-leaflet-overlay-container" color="blue" onClick={() => this.setState({ showWizard: true })}>
          Add Marker
        </Button>
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
