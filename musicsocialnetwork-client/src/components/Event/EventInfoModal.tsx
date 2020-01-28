import React from 'react';
import { Modal, Button, Icon, Grid, Label } from 'semantic-ui-react';
import { EventData } from '../../models/Event';
import LinkList from '../../common/LinkList';

interface EventInfoProps {
  event: EventData;
}

export default class EventInfoModal extends React.Component<EventInfoProps> {
  public render() {
    const event = this.props.event;

    return (
      <Modal trigger={
        <Button size="mini" color="grey" icon><Icon name="angle right" size="big" title="Details" /></Button>
      } closeIcon>
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
                  <Label>Date</Label><span className="info">{event.date.toLocaleDateString('hr-HR')}</span>
                </div>
                <div className="info-row">
                  <Label>Venue</Label><span className="info">{event.venue}</span>
                </div>
                <div className="info-row">
                  <Label>Headliners</Label><span className="info"><LinkList artists={[]} /></span>
                </div>
                <div className="info-row">
                  <Label>Supporters</Label><span className="info"><LinkList artists={[]} /></span>
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
    );
  }
}