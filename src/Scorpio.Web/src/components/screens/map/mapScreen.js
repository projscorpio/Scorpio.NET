import React, {Component, useState} from "react";
import {withRouter} from "react-router-dom";
import MapWidget from "./mapWidget";



class MapScreen extends Component {
  render() {
    return <MapWidget/>
  }
}


export default withRouter(MapScreen);
