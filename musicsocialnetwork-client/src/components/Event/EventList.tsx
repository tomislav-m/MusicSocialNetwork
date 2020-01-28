import React from 'react';
import { EventData } from '../../models/Event';
import { Table, Modal, Button, Icon } from 'semantic-ui-react';
import './EventList.css';
import CreateEditEvent from './CreateEditEvent';
import EventInfoModal from './EventInfoModal';
import LinkList from '../../common/LinkList';

interface EventProps {
  events: Array<EventData> | undefined;
  simpleArtistsDict: { [id: number]: string } | undefined;
}

export default class EventList extends React.Component<EventProps> {
  render() {
    const events = this.props.events;
    const dict = this.props.simpleArtistsDict || {};

    return (
      <div>
        <Button icon="plus" size="mini" color="green" title="Add event" />
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
                  <Table.Cell>{event.date.toLocaleString('hr-HR')}</Table.Cell>
                  <Table.Cell>{event.venueName}</Table.Cell>
                  <Table.Cell><LinkList artists={event.headliners.map(x => { return { id: event.id, name: dict[x] }; })} /></Table.Cell>
                  <Table.Cell><LinkList artists={event.supporters.map(x => { return { id: event.id, name: dict[x] }; })} /></Table.Cell>
                  <Table.Cell>
                    <Modal trigger={<Button icon compact><Icon name="edit" /></Button>}>
                      <Modal.Header>Edit event</Modal.Header>
                      <Modal.Content>
                        <CreateEditEvent oldEvent={event} isEdit={true} onEventSave={() => null} />
                      </Modal.Content>
                    </Modal>
                  </Table.Cell>
                  <Table.Cell>
                    <EventInfoModal event={event} />
                  </Table.Cell>
                </Table.Row>
              )
            }
          </Table.Body>
        </Table>
      </div>
    );
  }
}