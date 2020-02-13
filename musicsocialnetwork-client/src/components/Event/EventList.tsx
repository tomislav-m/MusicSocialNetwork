import React from 'react';
import { EventData, UserEvent } from '../../models/Event';
import { Table, Modal, Button, Icon } from 'semantic-ui-react';
import './EventList.css';
import CreateEditEvent from './CreateEditEvent';
import EventInfoModal from './EventInfoModal';
import LinkList from '../../common/LinkList';
import autobind from 'autobind-decorator';
import { createEvent, editEvent } from '../../actions/Events/EventActions';
import ArtistStore from '../../stores/ArtistStore';

interface EventProps {
  simpleArtistsDict: { [id: number]: string } | undefined;
  artistId: number;
  store: ArtistStore | undefined;
  userId: number | undefined;
  userEvents: Array<UserEvent>;
}

export default class EventList extends React.Component<EventProps> {
  render() {
    const events = this.props.store?.events;
    const dict = this.props.simpleArtistsDict || {};
    const artistId = this.props.artistId;

    return (
      <div>
        <Modal trigger={<Button icon="plus" size="mini" color="green" title="Add event" />}>
          <Modal.Header>Edit event</Modal.Header>
          <Modal.Content>
            <CreateEditEvent isEdit={false} headliner={{id: artistId, name: dict[artistId]}} onEventSave={this.handleCreateEvent} />
          </Modal.Content>
        </Modal>
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
                <Table.Row key={event.id}>
                  <Table.Cell>{(new Date(event.date)).toLocaleDateString('hr-HR')}</Table.Cell>
                  <Table.Cell>{event.venue}</Table.Cell>
                  <Table.Cell><LinkList artists={event.headliners.map(x => { return { id: event.id, name: dict[x] }; })} /></Table.Cell>
                  <Table.Cell><LinkList artists={event.supporters.map(x => { return { id: event.id, name: dict[x] }; })} /></Table.Cell>
                  <Table.Cell>
                    <Modal trigger={<Button icon compact><Icon name="edit" /></Button>}>
                      <Modal.Header>Edit event</Modal.Header>
                      <Modal.Content>
                        <CreateEditEvent headliner={{id: artistId, name: dict[artistId]}} oldEvent={event} isEdit={true} onEventSave={this.handleEditEvent} />
                      </Modal.Content>
                    </Modal>
                  </Table.Cell>
                  <Table.Cell>
                    <EventInfoModal event={event} userId={this.props.userId} userEvent={this.props.userEvents.filter(x => x.eventId === event.id)[0]} />
                  </Table.Cell>
                </Table.Row>
              )
            }
          </Table.Body>
        </Table>
      </div>
    );
  }

  @autobind
  handleCreateEvent(event: EventData) {
    createEvent(event)
      .then((data: EventData) => {
        this.props.store?.events.push(data);
      });
  }

  @autobind
  handleEditEvent(event: EventData) {
    editEvent(event)
      .then((data: EventData) => {
        let eventData = this.props.store?.events?.find(x => x.id === event.id);
        if (eventData) {
          eventData = { ...eventData };
        }
      });
  }
}