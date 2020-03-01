export const convertBase64DescriptionToRenderableHtml = base64 => {
  let html = { __html: "", hasValue: false };

  if (base64) {
    const rawHtml = atob(base64);
    const parser = new DOMParser();
    const doc = parser.parseFromString(rawHtml, "text/html");

    // remove headings
    const headings = doc.evaluate("/html/body//h1", doc, null, XPathResult.ANY_TYPE, null);
    const currentHeading = headings.iterateNext();
    if (currentHeading) currentHeading.parentElement.removeChild(currentHeading);

    // remove references
    const refs = doc.evaluate("//*[text() = 'Cross reference']/..", doc, null, XPathResult.ANY_TYPE, null);
    const currentRef = refs.iterateNext();
    if (currentRef) currentRef.parentElement.removeChild(currentRef);

    html.__html = doc.documentElement.innerHTML;
    html.hasValue = true;
  }
  return html;
};

export const mapStateTreeToRenderableTree = stateTree => {
  return stateTree
    ? stateTree.map(x => {
        return {
          key: `_${x.index}`,
          label: x.name,
          _index: x.index,
          nodes: x.subItems.map(sub => {
            return {
              label: sub.name,
              key: `${x.index}__${sub.index}`,
              _index: x.index,
              _subIndex: sub.index
            };
          })
        };
      })
    : [];
};
