import React, { useEffect, useState } from "react";
import { TOPICS } from "../../../../constants/appConstants";
import MessagingService from "../../../../services/MessagingService";
import LogService from "../../../../services/LogService";
import { Segment } from "semantic-ui-react";

const UbiquitiWidget = () => {
  const [lastUpdated, setLastUpdated] = useState(null);
  const [data, setData] = useState(null);

  const websocketHandler = data => {
    LogService.debug(`UbiquitiW idget received data`, data);

    if (data) {
      try {
        const parsed = JSON.parse(data);
        setLastUpdated(new Date());
        setData(parsed);
      } catch {}
    }
  };

  useEffect(() => {
    MessagingService.subscribe(TOPICS.UBIQUITI_DATA, websocketHandler); // sub on mount

    return () => {
      MessagingService.unsubscribe(TOPICS.UBIQUITI_DATA, websocketHandler); // unsub on unmount
    };
  });

  return (
    <Segment basic>
      {data ? (
        <ul style={{ padding: 0, listStyle: "none", marginTop: 0 }}>
          {Object.keys(data).map(key => {
            return (
              <li style={{ fontWeight: 600 }} key={key}>
                {key}: {data[key]}
              </li>
            );
          })}
        </ul>
      ) : (
        "No ubiquiti data recieved yet"
      )}
      {lastUpdated && <span>Last Updated: {lastUpdated.toLocaleTimeString()}</span>}
    </Segment>
  );
};

export default UbiquitiWidget;
