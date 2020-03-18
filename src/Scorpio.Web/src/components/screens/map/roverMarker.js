import React from "react";
import { useSelector } from "react-redux";
import { Icon as LeafletIcon } from "leaflet";
import RotatedMarker from "./rotatedMarker";

const RoverMarker = () => {
  const iconSizes = 30;
  const stateOfMap = useSelector(x => x.map);
  const { roverPosition, roverRotation } = stateOfMap;

  if (!roverPosition.latitude || !roverPosition.longitude) return null;

  return (
    <RotatedMarker
      position={[roverPosition.latitude, roverPosition.longitude]}
      rotationAngle={roverRotation - 45} // cause of angled arrow
      icon={
        new LeafletIcon({
          iconUrl: "/map/rover-icon-default.png",
          iconSize: [iconSizes, iconSizes]
        })
      }
    />
  );
};

export default RoverMarker;
