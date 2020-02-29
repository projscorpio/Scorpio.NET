import React, { useEffect, useState } from "react";
import Spinner from "../../common/spinner";
import { genericApi } from "../../../api/genericApi";
import { API } from "../../../constants/appConstants";
import { convertBase64DescriptionToRenderableHtml } from "./htmlHelper";

const CanOpenDescription = ({ canObject }) => {
  const [param, setParam] = useState({});
  const [showSpinner, setShowSpinner] = useState(false);

  useEffect(() => {
    const update = async () => {
      await fetchRenderObject(canObject.index, canObject.subIndex);
    };

    if (canObject && canObject.index) update();
  }, [canObject]);

  const fetchRenderObject = async (index, subIndex) => {
    setShowSpinner(true);
    const resp = await genericApi(API.CAN_OPEN.GET_OBJECT.format(index, subIndex), "GET");
    setParam(resp.body.data);
    setShowSpinner(false);
  };

  const htmlDesc = convertBase64DescriptionToRenderableHtml(param.base64Desc);
  return (
    <>{showSpinner ? <Spinner /> : <>{htmlDesc.hasValue ? <div dangerouslySetInnerHTML={htmlDesc} /> : <span>No description</span>}</>}</>
  );
};

export default CanOpenDescription;
