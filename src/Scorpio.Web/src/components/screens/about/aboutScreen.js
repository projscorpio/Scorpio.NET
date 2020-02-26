import React, { Component } from "react";
import { Grid, Header, Image } from "semantic-ui-react";
import { genericApi } from "../../../api/genericApi";
import { API } from "../../../constants/appConstants";

class AboutScreen extends Component {
  constructor(props) {
    super(props);
    this.state = { apiInfo: null };
  }

  async componentDidMount() {
    const result = await genericApi(API.ROOT, "GET");
    if (result && result.response && result.response.ok) {
      this.setState({ apiInfo: result.body });
    }
  }
  render() {
    const mode = process.env.NODE_ENV;
    const publicUrl = process.env.PUBLIC_URL;
    let apiEndpoint = process.env.REACT_APP_BACKEND_URL;
    if (!apiEndpoint) apiEndpoint = "/";
    let { apiInfo } = this.state;
    const swaggerUrl = process.env.REACT_APP_BACKEND_URL + "/swagger";
    if (apiInfo) apiInfo = JSON.stringify(apiInfo, null, 1);

    return (
      <div style={{ marginTop: "80px" }}>
        <Grid textAlign="center" style={{ height: "100%", maxWidth: "500px", margin: "auto" }} verticalAlign="middle">
          <Grid.Row>
            <Header as="h1">
              <div> Built env: {mode}</div> <div>API endpoint: {apiEndpoint}</div>
            </Header>
          </Grid.Row>
          <Grid.Row>
            <Header as="h1">Scorpio rover control App</Header>
          </Grid.Row>
          <Grid.Row>
            <Header>Created by: Mateusz Kryszczak</Header>
          </Grid.Row>
          <Grid.Row>
            <a href={swaggerUrl} rel="noopener noreferrer" target="_blank">
              <Image size="medium" src={publicUrl + "/swagger.png"} />
            </a>
          </Grid.Row>
          <Grid.Row>
            <div>Api info:</div>
            <pre style={{ backgroundColor: "lightgray" }}>{apiInfo}</pre>
          </Grid.Row>
        </Grid>
      </div>
    );
  }
}

export default AboutScreen;
