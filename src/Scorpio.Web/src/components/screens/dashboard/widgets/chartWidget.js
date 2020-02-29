import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import Chart from "../../scienceCharts/chart";
import { API, TOPICS } from "../../../../constants/appConstants";
import { genericApi } from "../../../../api/genericApi";
import Spinner from "../../../common/spinner";
import MessagingService from "../../../../services/MessagingService";
import LogService from "../../../../services/LogService";

const ChartWidget = ({ sensorKey }) => {
  const [isFetching, setIsFetching] = useState(true);
  const [data, setData] = useState([]);

  const sensors = useSelector(x => x.sensors);
  const sensor = sensors.find(x => x.sensorKey === sensorKey);

  const websocketHandler = msg => {
    LogService.debug(`Statistic ${sensorKey} received data`, msg);

    try {
      if (msg) {
        const parsed = JSON.parse(msg);
        if (parsed) {
          setData(data => data.concat(parsed));
        }
      }
    } catch {}
  };

  useEffect(() => {
    MessagingService.subscribe(TOPICS.SENSOR_DATA, websocketHandler);

    const endpoint = API.SENSOR_DATA.GET_PAGED_FILTERED.format(sensorKey, 1, 1000);
    genericApi(endpoint, "GET").then(result => {
      if (result.response && result.response.ok) {
        setIsFetching(false);
        setData(result.body.values);
      }
    });

    return () => {
      MessagingService.unsubscribe(TOPICS.SENSOR_DATA, websocketHandler);
    };
  }, [sensorKey]);

  const showSpinner = isFetching || sensor === undefined;
  return showSpinner ? <Spinner /> : <Chart sensor={sensor} data={data} />;
};

export default ChartWidget;
