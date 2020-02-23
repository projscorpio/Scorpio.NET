import React, { Component, useState } from "react";
import { withRouter } from "react-router-dom";
import { Message, Segment, Accordion, Icon, Form as SemanticForm, FormField, Button } from "semantic-ui-react";

import 'leaflet/dist/leaflet.css';
import "./leafletCssWebpakWorkaround"

import { Map, Marker, Tooltip, TileLayer, withLeaflet } from 'react-leaflet'
import GenericWizard from "../../../common/genericWizard";
import { Field } from "react-final-form";
import Validators from "../../../../utils/formValidators";
import CircleMarker from "react-leaflet/es/CircleMarker";
import RotatedMarker from "./RotatedMarker"
import { Icon as LeafletIcon } from "leaflet"
import * as actions from "../../../../actions";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";


const center = [51.107883, 17.038538];
const initZoom = 13;
const iconSizes = 20;


class MapWidget extends Component {
  getRoverIcon = (rotation) => {
    if (rotation === true) {
      const rotationCorrection = -45;
      const initRotation = 0;
      return <RotatedMarker id="test123"
                            position={[this.state.roverPosition.latitude, this.state.roverPosition.longitude]}
                            rotationAngle={rotationCorrection + initRotation}
                            icon={new LeafletIcon({
                              iconUrl: '/rover-icon-default.png',
                              iconSize: [iconSizes, iconSizes]
                            })} />;
    }
    return <CircleMarker center={[this.state.roverPosition.latitude, this.state.roverPosition.longitude]}
                         radius={iconSizes} />;

  }

  constructor(props) {
    super(props);
    console.log("props");
    console.log(props);
    this.state = { markers: this.props.markers, popup: false, roverPosition: this.props.roverPosition }
  }

  componentDidMount(props) {
    console.log("this.state");
    console.log(this.state);
    this.updateJob = setInterval(this.props.actions.fetchRoverPosition, 2000);
    this.props.actions.setMapMarkers(this.state.markers);
  }

  componentWillUnmount() {
    clearInterval(this.updateJob);
  }

  renderMarkers = () => {
    return this.state.markers.map((d) => <Marker position={[parseFloat(d.latitude), parseFloat(d.longitude)]}> <Tooltip><span
      style={{ fontWeight: "bold" }}>{d.name}</span> <br /> Latlong: &#91;{d.latitude}, {d.longitude}&#93;</Tooltip>
    </Marker>);
  };
  addLocation = (data) => {
    this.setState({
      popup: false,
      markers: [...this.state.markers, { name: data.Name, latitude: data.Lat, longitude: data.Long }]
    }, () => this.props.actions.setMapMarkers(this.state.markers))

  };

  render() {
    return <>
      {this.state.popup && <GenericWizard
        title={"Add location"}
        onClose={() => {
          this.setState({ popup: false })
        }}
        onSubmit={this.addLocation}
        showSteps={false}
      >
        <GenericWizard.Page title="Add Marker">
          <Field name="Name" validate={Validators.required}>
            {({ input, meta }) => (
              <SemanticForm.Input
                {...input}
                label="Name:"
                error={meta.invalid && meta.touched && meta.error}
                placeholder="Name..."
                required
                onChange={(ev, data) => input.onChange(data.value)}
              />
            )}
          </Field>
          <SemanticForm.Group widths='equal'>
            <Field name="Lat" validate={Validators.required}>
              {({ input, meta }) => (
                <SemanticForm.Input
                  {...input}
                  label="Lat:"
                  error={meta.invalid && meta.touched && meta.error}
                  placeholder="Lat..."
                  required
                  onChange={(ev, data) => input.onChange(data.value)}
                />
              )}
            </Field>
            <Field name="Long">
              {({ input, meta }) => (
                <SemanticForm.Input
                  {...input}
                  label="Long:"
                  error={meta.invalid && meta.touched && meta.error}
                  placeholder="Long..."
                  onChange={(ev, data) => input.onChange(data.value)}
                />
              )}
            </Field>
          </SemanticForm.Group>
        </GenericWizard.Page>
      </GenericWizard>}
      <Map className="map-style" style={{
        width: "100%", height: "100%", zIndex: "0"
      }} center={center}
           zoom={initZoom}>
        <TileLayer url="/wroc-map/{z}/{x}/{y}.png" />
        {this.renderMarkers()}
        {this.getRoverIcon(true)}
      </Map>
      <Button className="scorpio-leaflet-overlay-container mrg-right-auto mrg-left-sm" color="blue"
              onClick={() => this.setState({ popup: true })}>
        Add Marker
      </Button>
    </>
      ;
  }
}

function mapStateToProps(state) {
  console.log("state");
  console.log(state);
  return { markers: state.map.markers, roverPosition: state.map.roverPosition };
}

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(MapWidget));
