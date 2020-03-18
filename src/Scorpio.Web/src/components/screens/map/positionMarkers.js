import React from "react";
import { useSelector } from "react-redux";
import { Marker, Tooltip } from "react-leaflet";
import moment from "moment";
import { Button, Popup } from "semantic-ui-react";

const PositionMarkers = ({ onEditClicked }) => {
  const markers = useSelector(x => x.map).markers;

  const onEditClick = (ev, marker) => {
    ev.preventDefault();
    onEditClicked(marker);
  };

  return (
    <>
      {markers.map((m, indx) => (
        <Marker
          position={[parseFloat(m.latitude), parseFloat(m.longitude)]}
          key={`${m.latitude}_${m.longitude}_${indx}`}
          onMouseOver={e => e.target.openPopup()}
          onMouseOut={e => e.target.closePopup()}
        >
          <Popup>POPUP</Popup>
          <Tooltip interactive permanent>
            <span style={{ fontWeight: "bold" }}>
              {m.name} {moment(m.timeStamp).format(moment.HTML5_FMT.DATETIME_LOCAL)}
            </span>
            <br /> Lat: &#91;{m.latitude}, Lon: {m.longitude}&#93;
            <br />
            <center>
              <Button onClick={ev => onEditClick(ev, m)}>Edit</Button>
            </center>
          </Tooltip>
        </Marker>
      ))}
    </>
  );
};

export default PositionMarkers;
