import React, { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Grid } from "semantic-ui-react";
import * as canOpenActions from "../../../actions/canOpenActions";
import { genericApi } from "../../../api/genericApi";
import { API } from "../../../constants/appConstants";
import TreeMenu from "react-simple-tree-menu";
import "../../../../node_modules/react-simple-tree-menu/dist/main.css";
import { mapStateTreeToRenderableTree } from "./htmlHelper";
import CanOpenDescription from "./canOpenDescription";

const CanOpenExplorer = () => {
  const canOpenState = useSelector(x => x.canOpen);
  const dispatch = useDispatch();
  const [nodes, setNodes] = useState([]);
  const [canObject, setCanObject] = useState(null);

  useEffect(() => {
    if (canOpenState.tree.length === 0) {
      genericApi(API.CAN_OPEN.GET_TREE, "GET").then(resp => {
        dispatch(canOpenActions.setCanOpenTree(resp.body.data.items));
      });
    } else {
      const mappedTree = mapStateTreeToRenderableTree(canOpenState.tree);
      setNodes(mappedTree);
    }
  }, [canOpenState]);

  const onClickItem = async node => {
    const index = node._index;
    const subIndex = node._subIndex ? node._subIndex : "00";
    setCanObject({ index, subIndex });
  };

  return (
    <Grid stackable>
      <Grid.Row>
        <Grid.Column mobile={16} tablet={6} computer={4}>
          <div style={{ maxHeight: "95vh", overflowY: "scroll" }}>
            <TreeMenu data={nodes} onClickItem={onClickItem} />
          </div>
        </Grid.Column>
        <Grid.Column tablet={10} computer={12}>
          <Grid.Row>
            <CanOpenDescription canObject={canObject} />
          </Grid.Row>
        </Grid.Column>
      </Grid.Row>
    </Grid>
  );
};

export default CanOpenExplorer;
