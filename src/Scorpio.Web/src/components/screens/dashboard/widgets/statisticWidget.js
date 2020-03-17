import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { API, TOPICS } from "../../../../constants/appConstants";
import { genericApi } from "../../../../api/genericApi";
import Spinner from "../../../common/spinner";
import MessagingService from "../../../../services/MessagingService";
import LogService from "../../../../services/LogService";
import { Statistic } from "semantic-ui-react";
import moment from "moment";

const StatisticWidget = ({ sensorKey }) => {
  const [isFetching, setIsFetching] = useState(true);
  const [data, setData] = useState({ timeStamp: null, value: null });

  const sensors = useSelector(x => x.sensors);
  const sensor = sensors.find(x => x.sensorKey === sensorKey);

  const websocketHandler = data => {
    LogService.debug(`Statistic ${sensorKey} received data`, data);

    try {
      if (data) {
        const parsed = JSON.parse(data);
        if (parsed) {
          processData(parsed);
        }
      }
    } catch {}
  };

  const processData = value => {
    let v = value || { timeStamp: null, value: null };
    const parsedValue = Number.parseFloat(v.value);
    if (!isNaN(parsedValue)) v.value = parsedValue.toFixed(3);
    setData(v);
  };

  useEffect(() => {
    MessagingService.subscribe(TOPICS.SENSOR_DATA, websocketHandler);

    const endpoint = API.SENSOR_DATA.GET_LATEST.format(sensorKey);
    genericApi(endpoint, "GET").then(result => {
      if (result.response && result.response.ok) {
        setIsFetching(false);
        processData(result.body);
      }
    });

    return () => {
      MessagingService.unsubscribe(TOPICS.SENSOR_DATA, websocketHandler);
    };
  }, [sensorKey]);

  const showSpinner = isFetching || sensor === undefined;
  return showSpinner ? (
    <Spinner />
  ) : (
    <Statistic>
      {data.value ? (
        <>
          <Statistic.Value text>
            {sensor.name}
            <br />
            {`${data.value} ${sensor.unit}`}
          </Statistic.Value>
          <Statistic.Label>{moment(data.timeStamp).format("MM dd, h:mm:ss a")}</Statistic.Label>
        </>
      ) : (
        "No data available"
      )}
    </Statistic>
  );
};

export default StatisticWidget;
