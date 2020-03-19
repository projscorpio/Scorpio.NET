import React from "react";
import { useSelector } from "react-redux";
import { Marker, Popup } from "react-leaflet";
import { Button } from "semantic-ui-react";
import moment from "moment";

const PositionMarkers = ({ onEditClicked, onRemoveClicked }) => {
  const { markers } = useSelector(x => x.map);

  const onEditClick = (ev, marker) => {
    ev.preventDefault();
    onEditClicked(marker);
  };

  const onDragEnd = (ev, marker) => {
    const { _latlng } = ev.target;
    onEditClicked({ ...marker, latitude: _latlng.lat, longitude: _latlng.lng });
  };

  return (
    <>
      {markers.map((m, indx) => (
        <Marker
          draggable
          ondragend={ev => onDragEnd(ev, m)}
          position={[parseFloat(m.latitude), parseFloat(m.longitude)]}
          key={`${m.latitude}_${m.longitude}_${indx}`}
        >
          <Popup>
            <span style={{ fontWeight: "bold" }}>
              {m.name} {moment(m.timeStamp).format(moment.HTML5_FMT.DATETIME_LOCAL)}
            </span>
            <br /> Lat: &#91;{m.latitude}, Lon: {m.longitude}&#93;
            <div style={{ textAlign: "center", marginTop: "0.8em" }}>
              <Button icon="pencil" color="vk" onClick={ev => onEditClick(ev, m)} />
              <Button icon="close" color="red" onClick={ev => onRemoveClicked(ev, m)} />
            </div>
          </Popup>
        </Marker>
      ))}
    </>
  );
};

export default PositionMarkers;
