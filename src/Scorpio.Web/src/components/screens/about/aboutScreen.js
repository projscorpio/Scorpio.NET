import React, { Component } from "react";
import { Grid, Header, Image, Segment, Container, Icon } from "semantic-ui-react";
import { genericApi } from "../../../api/genericApi";
import { API } from "../../../constants/appConstants";
import { Link } from "react-router-dom";
import AppHealth from "../../common/appHealth";

class AboutScreen extends Component {
  constructor(props) {
    super(props);
    this.state = { apiInfo: null };
  }

  async componentDidMount() {
    const result = await genericApi(API.HOME, "GET");
    if (result && result.response && result.response.ok) {
      this.setState({ apiInfo: result.body });
    }
  }

  render() {
    const mode = process.env.NODE_ENV;
    const publicUrl = process.env.PUBLIC_URL;
    let { apiInfo } = this.state;
    if (apiInfo) apiInfo = JSON.stringify(apiInfo, null, 1);

    return (
      <>
        <Segment textAlign="center">
          <Header>App health</Header>
          <Container fluid>
            <AppHealth />
          </Container>
        </Segment>

        <Segment>
          <Grid textAlign="center" style={{ height: "100%", maxWidth: "500px", margin: "auto" }} verticalAlign="middle">
            <Grid.Row textAlign="center">
              <Header as="h3">SPA info:</Header>
            </Grid.Row>
            <Grid.Row className="padding-top-0 margin-top-0">
              <pre style={{ backgroundColor: "lightgray" }}>
                Built env: {mode}
                <br /> API endpoint: {API.ROOT}
              </pre>
            </Grid.Row>
            <Grid.Row className="padding-top-0 margin-top-0">
              <Header as="h3">Api info:</Header>
            </Grid.Row>
            <Grid.Row>
              <a href={API.SWAGGER} rel="noopener noreferrer" target="_blank">
                <Image size="small" src={publicUrl + "/swagger.png"} />
              </a>
              <pre style={{ backgroundColor: "lightgray" }}>{apiInfo}</pre>
            </Grid.Row>
            <Grid.Row>
              <Header as="h3">Created by: Mateusz Kryszczak</Header>
            </Grid.Row>
          </Grid>
        </Segment>

        <Segment>
          <Header>Experimental / Obsolete / Descoped features</Header>
          <Container fluid>
            <Grid columns={3} stackable textAlign="center">
              <Grid.Row verticalAlign="middle">
                <Grid.Column>
                  <Link to="/stream">
                    <Header icon>
                      <Icon name="video" />
                      Stream
                    </Header>
                  </Link>
                </Grid.Column>
                <Grid.Column>
                  <Link to="/gamepad">
                    <Header icon>
                      <Icon name="gamepad" />
                      Joystick control
                    </Header>
                  </Link>
                </Grid.Column>
                <Grid.Column>
                  <Link to="/filu">
                    <Header icon>
                      <Icon name="wheelchair" />
                      Filu Racer
                    </Header>
                  </Link>
                </Grid.Column>
              </Grid.Row>
            </Grid>
          </Container>
        </Segment>
      </>
    );
  }
}

export default AboutScreen;
