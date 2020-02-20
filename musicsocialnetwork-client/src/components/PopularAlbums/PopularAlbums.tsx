import React from 'react';
import { Grid, Table } from 'semantic-ui-react';

interface PopularAlbumsState {
  todayAlbums: Array<any>;
  weekAlbums: Array<any>;
  monthAlbums: Array<any>;
}

export default class PopularAlbums extends React.Component<{}, PopularAlbumsState> {

  constructor() {
    super({});

    this.state = {
      todayAlbums: [],
      weekAlbums: [],
      monthAlbums: []
    };
  }

  render() {
    const { todayAlbums, weekAlbums, monthAlbums } = this.state;

    return (
      <Grid>
        <Grid.Row>
          <Grid.Column width="5">
            <Table striped>
              <Table.Header>
                <Table.Row>
                  <Table.HeaderCell>Today</Table.HeaderCell>
                </Table.Row>
              </Table.Header>
              <Table.Body>
                {
                  todayAlbums.map(album =>
                    <Table.Row>
                      <Table.Cell>{}</Table.Cell>
                      <Table.Cell>{}</Table.Cell>
                    </Table.Row>
                  )
                }
              </Table.Body>
            </Table>
          </Grid.Column>
          <Grid.Column width="5">
            <Table striped>
              <Table.Header>
                <Table.Row>
                  <Table.HeaderCell>Week</Table.HeaderCell>
                </Table.Row>
              </Table.Header>
              <Table.Body>
                {
                  todayAlbums.map(album =>
                    <Table.Row>
                      <Table.Cell>{}</Table.Cell>
                      <Table.Cell>{}</Table.Cell>
                    </Table.Row>
                  )
                }
              </Table.Body>
            </Table>
          </Grid.Column>
          <Grid.Column width="5">
            <Table striped>
              <Table.Header>
                <Table.Row>
                  <Table.HeaderCell>Month</Table.HeaderCell>
                </Table.Row>
              </Table.Header>
              <Table.Body>
                {
                  todayAlbums.map(album =>
                    <Table.Row>
                      <Table.Cell>{}</Table.Cell>
                      <Table.Cell>{}</Table.Cell>
                    </Table.Row>
                  )
                }
              </Table.Body>
            </Table>
          </Grid.Column>
        </Grid.Row>
      </Grid>
    );
  }
}