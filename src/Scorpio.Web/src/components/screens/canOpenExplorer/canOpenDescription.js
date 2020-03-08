import React, { useEffect, useState } from "react";
import Spinner from "../../common/spinner";
import { genericApi } from "../../../api/genericApi";
import { convertBase64DescriptionToRenderableHtml } from "./htmlHelper";
import Collapsible from "../../common/collapsible";
import CanOpenTryItEditor from "./canOpenTryItEditor";
import { Segment, Header, Message } from "semantic-ui-react";
import { ObjectAsListRenderer } from "../../common/listRenderer";
import { getKeyByValue, dataTypeDict } from "./canOpenDictionaries";
import { withRouter } from "react-router-dom";

const CanOpenDescription = ({ location, emptyMessage, publishUrl, getUrl }) => {
  const [param, setParam] = useState({});
  const [showSpinner, setShowSpinner] = useState(false);

  useEffect(() => {
    const queryParams = new URLSearchParams(location.search);
    const index = queryParams.get("index");
    const subIndex = queryParams.get("subIndex");

    const update = async () => {
      await fetchRenderObject(index, subIndex);
    };

    if (index && subIndex) update();
  }, [location]);

  const fetchRenderObject = async (index, subIndex) => {
    setShowSpinner(true);
    const resp = await genericApi(getUrl.format(index, subIndex), "GET");
    setParam(resp.body.data);
    setShowSpinner(false);
  };

  const onTryItSubmit = async data => {
    await genericApi(publishUrl, "POST", data);
  };

  const htmlDesc = convertBase64DescriptionToRenderableHtml(param.base64Desc);

  return (
    <>
      {showSpinner ? (
        <Spinner />
      ) : (
        <>
          {param.index ? (
            <>
              <Segment color="orange">
                <Header as="h3">
                  {param.name} - {param.index}:{param.subIndex ? param.subIndex : "00"}
                </Header>
              </Segment>
              <Collapsible title="Try it out" initialExpanded>
                <CanOpenTryItEditor
                  index={param.index}
                  subIndex={param.subIndex ? param.subIndex : "00"}
                  dataType={param.dataType}
                  onTryItSubmit={onTryItSubmit}
                />
              </Collapsible>
              <Collapsible title="Properties" initialExpanded={false}>
                <ObjectAsListRenderer
                  object={param}
                  keysToSkip={["base64Desc"]}
                  dictionaryResolvers={{ dataType: (key, value) => getKeyByValue(dataTypeDict, value) }}
                  keyTransforms={["upper"]}
                />
              </Collapsible>
              {htmlDesc.hasValue ? (
                <Collapsible title={"Original description"} initialExpanded>
                  <div dangerouslySetInnerHTML={htmlDesc} />
                </Collapsible>
              ) : (
                "No description available"
              )}
            </>
          ) : (
            <Message>{emptyMessage}</Message>
          )}
        </>
      )}
    </>
  );
};

export default withRouter(CanOpenDescription);
