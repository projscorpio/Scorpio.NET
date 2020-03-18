import React, { useEffect, useState } from "react";
import { Icon } from "semantic-ui-react";
import MessagingService from "../../services/MessagingService";
import { genericApi } from "../../api/genericApi";
import { API } from "../../constants/appConstants";

const SignleState = ({ resource, isOk }) => {
  return (
    <div className="text-large" style={{ marginTop: "4px" }}>
      <Icon color={isOk ? "green" : "red"} name="heartbeat"></Icon>
      {resource}: <span className={isOk ? "ok" : "nok"}>{isOk ? "OK" : "NOK"}</span>
    </div>
  );
};

const AppHealth = () => {
  const [isSignalrOk, setSignalrOk] = useState(MessagingService.isConnected());
  const [apiStatuses, setApiStatuses] = useState([]);

  const signalRStateChanged = state => {
    console.log(state);
    setSignalrOk(state === "Connected");
  };

  useEffect(() => {
    const pollApiStatuses = async () => await genericApi(API.HEALTH, "GET");
    const updateApiStatuses = async () => {
      MessagingService.subscribeConnectionChange(signalRStateChanged);

      try {
        const response = await pollApiStatuses();
        if (response && response.body && response.body.results) {
          const statuses = Object.keys(response.body.results).map(key => {
            const val = response.body.results[key];
            return { resource: key, isOk: val.isHealthy };
          });
          setApiStatuses(statuses);
        }
      } catch (err) {
        setApiStatuses(statuses =>
          statuses.map(x => {
            return {
              ...x,
              isOk: false
            };
          })
        );
      }
    };

    updateApiStatuses();

    const pollerTask = setInterval(updateApiStatuses, 10000);

    // When unmount
    return () => {
      clearInterval(pollerTask);
      MessagingService.unSubscribeConnectionChange(signalRStateChanged);
    };
  }, []);

  return (
    <>
      <SignleState resource={"SignalR"} isOk={isSignalrOk} />
      {apiStatuses.map(x => {
        return <SignleState key={x.resource} resource={x.resource} isOk={x.isOk} />;
      })}
    </>
  );
};

export default AppHealth;
