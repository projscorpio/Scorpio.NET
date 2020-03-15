import React, { Component } from "react";
import { Grid, Header, Image, Segment, Container, Icon } from "semantic-ui-react";
import { genericApi } from "../../../api/genericApi";
import { API } from "../../../constants/appConstants";
import { Link } from "react-router-dom";

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
                  <Link to="/map">
                    <Header icon>
                      <Icon name="map" />
                      Map
                    </Header>
                  </Link>
                </Grid.Column>
              </Grid.Row>
            </Grid>
          </Container>
        </Segment>
        <div style={{ marginTop: "80px" }}>
          <Grid textAlign="center" style={{ height: "100%", maxWidth: "500px", margin: "auto" }} verticalAlign="middle">
            <Grid.Row>
              <Header as="h1">
                <div> Built env: {mode}</div> <div>API endpoint: {API.ROOT}</div>
              </Header>
            </Grid.Row>
            <Grid.Row>
              <Header as="h1">Scorpio rover control App</Header>
            </Grid.Row>
            <Grid.Row>
              <Header>Created by: Mateusz Kryszczak</Header>
            </Grid.Row>
            <Grid.Row>
              <a href={API.SWAGGER} rel="noopener noreferrer" target="_blank">
                <Image size="medium" src={publicUrl + "/swagger.png"} />
              </a>
            </Grid.Row>
            <Grid.Row>
              <div>Api info:</div>
              <pre style={{ backgroundColor: "lightgray" }}>{apiInfo}</pre>
            </Grid.Row>
          </Grid>
        </div>
      </>
    );
  }
}

export default AboutScreen;
