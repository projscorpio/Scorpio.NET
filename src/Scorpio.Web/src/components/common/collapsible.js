import React, { useState } from "react";
import { Accordion, Icon } from "semantic-ui-react";

const Collapsible = ({ title, children, initialExpanded }) => {
  const [expanded, setExpanded] = useState(initialExpanded === undefined ? false : !!initialExpanded);
  const handleExpand = () => {
    setExpanded(!expanded);
  };

  return (
    <Accordion fluid styled className="margin-bottom-m">
      <Accordion.Title active={expanded} onClick={handleExpand}>
        <div className="inline">
          <Icon name="dropdown" />
          {title}
        </div>
      </Accordion.Title>
      <Accordion.Content active={expanded}>{children}</Accordion.Content>
    </Accordion>
  );
};

export default Collapsible;
