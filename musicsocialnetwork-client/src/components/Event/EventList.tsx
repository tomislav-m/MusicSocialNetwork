import React from 'react';
import { EventData } from '../../models/Event';
import { Table, Modal, Button, Icon, Grid, Label } from 'semantic-ui-react';
import './EventList.css';
import CreateEditEvent from './CreateEditEvent';
import { ArtistDataSimple } from '../../models/Artist';

interface EventProps {
  events: Array<EventData> | undefined;
}

export default class EventList extends React.Component<EventProps> {
  render() {
    const events = this.props.events;

    return (
      <Table striped compact>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>Date</Table.HeaderCell>
            <Table.HeaderCell>Venue</Table.HeaderCell>
            <Table.HeaderCell>Headliners</Table.HeaderCell>
            <Table.HeaderCell>Supporters</Table.HeaderCell>
            <Table.HeaderCell></Table.HeaderCell>
            <Table.HeaderCell></Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {
            events?.map(event =>
              <Table.Row key={event.Id}>
                <Table.Cell>{event.Date.toLocaleString('hr-HR')}</Table.Cell>
                <Table.Cell>{event.VenueName}</Table.Cell>
                <Table.Cell>{this.arrayToList(event.Headliners)}</Table.Cell>
                <Table.Cell>{this.arrayToList(event.Supporters)}</Table.Cell>
                <Table.Cell>
                  <Modal trigger={<Button icon compact><Icon name="edit" /></Button>}>
                    <Modal.Header>Edit event</Modal.Header>
                    <Modal.Content>
                      <CreateEditEvent oldEvent={event} isEdit={true} onEventSave={() => null} />
                    </Modal.Content>
                  </Modal>
                </Table.Cell>
                <Table.Cell>
                  <Modal trigger={<Button size="mini" color="grey" icon><Icon name="angle right" size="big" /></Button>} closeIcon>
                    <Modal.Header>Event</Modal.Header>
                    <Modal.Content>
                      <Grid>
                        <Grid.Row divided>
                          <Grid.Column width="5">
                            <img src="https://www.theaudiodb.com/images/media/artist/thumb/rtpxtt1385762058.jpg" alt="headliner" width="200" />
                            <img src="https://www.theaudiodb.com/images/media/artist/thumb/tqsvpt1363622181.jpg" alt="supporter" width="100" />
                          </Grid.Column>
                          <Grid.Column width="11">
                            <div className="info-row">
                              <Label>Date</Label><span className="info">{event.Date.toLocaleString('hr-HR')}</span>
                            </div>
                            <div className="info-row">
                              <Label>Venue</Label><span className="info">{event.VenueName}</span>
                            </div>
                            <div className="info-row">
                              <Label>Headliners</Label><span className="info">{this.arrayToList(event.Headliners)}</span>
                            </div>
                            <div className="info-row">
                              <Label>Headliners</Label><span className="info">{this.arrayToList(event.Supporters)}</span>
                            </div>
                          </Grid.Column>
                        </Grid.Row>
                      </Grid>
                    </Modal.Content>
                    <Modal.Actions>
                      <Button color="green">
                        <Icon name="check circle" /> Going!
                      </Button>
                      <Button color="orange">
                        <Icon name="question circle" /> Interested...
                      </Button>
                    </Modal.Actions>
                  </Modal>
                </Table.Cell>
              </Table.Row>
            )
          }
        </Table.Body>
      </Table>
    );
  }

  private arrayToList(array: Array<ArtistDataSimple>) {
    let list = '';
    array.map(x => x.Name).forEach((x, index) => {
      if (index > 0) {
        list += ',';
      }
      list += x;
    });

    return list;
  }
}