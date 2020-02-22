import React from 'react';
import { Modal, Button, Icon, Grid, Label, Input } from 'semantic-ui-react';
import { EventData, UserEvent } from '../../models/Event';
import LinkList from '../../common/LinkList';
import { getArtist } from '../../actions/Music/MusicActions';
import { defaultArtistDataSimple } from '../../models/Artist';
import { markEvent, buyTickets } from '../../actions/Events/EventActions';
import autobind from 'autobind-decorator';
import Comments from '../Comments/Comments';
import Notification from '../../common/Notification';
import './EventInfoModal.css';
import UserStore from '../../stores/UserStore';
import { observer, inject } from 'mobx-react';

interface EventInfoProps {
  event: EventData;
  userId: number | undefined;
  userEvent: UserEvent;
  userStore?: UserStore;
}

interface EventInfoState {
  artists: Array<any>;
  isGoingLoading: boolean;
  isInterestedLoading: boolean;
  isBuyingTicket: boolean;
  userEvent: UserEvent;
  ticketsCount: number;
  modalOpen: boolean;
}

@inject('userStore')
@observer
export default class EventInfoModal extends React.Component<EventInfoProps, EventInfoState> {
  constructor(props: EventInfoProps) {
    super(props);

    this.state = {
      artists: [],
      isGoingLoading: false,
      isInterestedLoading: false,
      userEvent: props.userEvent,
      isBuyingTicket: false,
      ticketsCount: 1,
      modalOpen: false
    };
  }

  componentDidMount() {
    const event = this.props.event;
    this.getArtists([...event.headliners, ...event.supporters]);
  }

  public render() {
    const { event } = this.props;
    const { userEvent, isBuyingTicket } = this.state;

    return (
      <Modal trigger={
        <Button size="mini" color="grey" icon onClick={() => this.setState({ modalOpen: true })}><Icon name="angle right" size="big" title="Details" /></Button>
      }
        onClose={() => this.setState({ modalOpen: false })}
        open={this.state.modalOpen}
        closeIcon>
        <Modal.Header>Event</Modal.Header>
        <Modal.Content>
          <Grid className="modal-grid">
            <Grid.Row divided>
              <Grid.Column width="5" className="event-images">
                {
                  event.headliners.map(artistId => {
                    const artist = this.state.artists.find(x => x.id === artistId);
                    return artist ? <img key={artistId} src={artist.photoUrl} alt="headliner" width="200" /> : <span key={artistId}></span>;
                  })
                }{
                  event.supporters.map(artistId => {
                    const artist = this.state.artists.find(x => x.id === artistId);
                    return artist ? <img key={artistId} src={artist.photoUrl} alt="supporter" width="100" /> : <span key={artistId}></span>;
                  })
                }
              </Grid.Column>
              <Grid.Column width="11" className="event-details">
                <div className="info-row">
                  <Label>Date</Label><span className="info">{(new Date(event.date)).toLocaleDateString('hr-HR')}</span>
                </div>
                <div className="info-row">
                  <Label>Venue</Label><span className="info">{event.venue}</span>
                </div>
                <div className="info-row">
                  <Label>Headliners</Label>
                  <span className="info">
                    <LinkList artists={event.headliners.map(id => {
                      const artist = this.state.artists.find(x => x.id === id) || defaultArtistDataSimple;
                      return { id, name: artist.name };
                    })} />
                  </span>
                </div>
                <div className="info-row">
                  <Label>Supporters</Label><span className="info">
                    <LinkList artists={event.supporters.map(id => {
                      const artist = this.state.artists.find(x => x.id === id) || defaultArtistDataSimple;
                      return { id, name: artist.name };
                    })} />
                  </span>
                </div>
                <Comments pageType="events" parentId={event.id} />
              </Grid.Column>
            </Grid.Row>
          </Grid>
        </Modal.Content>
        <Modal.Actions>
          <Button
            floated="left"
            onClick={this.handleBuyTickets}
            loading={isBuyingTicket}
          > Buy tickets</Button>
          <Input
            className="tickets-input"
            type="number"
            onChange={this.handleTicketsValueChange}
            value={this.state.ticketsCount}
          />
          <Button
            color={(userEvent && userEvent.markEventType === 0) ? 'green' : 'grey'}
            disabled={this.state.isGoingLoading}
            loading={this.state.isGoingLoading}
            onClick={() => this.handleMarkEvent(0)}>
            <Icon name="check circle" /> Going!
          </Button>
          <Button
            color={(userEvent && userEvent.markEventType === 1) ? 'orange' : 'grey'}
            disabled={this.state.isInterestedLoading}
            loading={this.state.isInterestedLoading}
            onClick={() => this.handleMarkEvent(1)}>
            <Icon name="question circle" /> Interested...
          </Button>
        </Modal.Actions>
      </Modal>
    );
  }

  private async getArtists(array: Array<number>) {
    for (const id of array) {
      const artist = await getArtist(id);
      this.setState({
        artists: [...this.state.artists, artist]
      });
    }
  }

  @autobind
  private handleTicketsValueChange(event: any, { value }: any) {
    this.setState({
      ticketsCount: value
    });
  }

  @autobind
  private handleMarkEvent(markEventType: number) {
    const { userId, event } = this.props;
    if (userId) {
      this.setLoading(markEventType);
      markEvent(userId, event.id, markEventType)
        .then(result => {
          if (!result.exception) {
            this.setState({
              userEvent: result
            });
          }
          this.setLoading(markEventType);
        });
    }
  }

  private setLoading(markEventType: number) {
    if (markEventType === 0) {
      this.setState({
        isGoingLoading: !this.state.isGoingLoading
      });
    } else {
      this.setState({
        isInterestedLoading: !this.state.isInterestedLoading
      });
    }
  }

  @autobind
  private handleBuyTickets() {
    const { userId, event } = this.props;
    const { ticketsCount } = this.state;

    if (userId && ticketsCount > 0) {
      this.setState({
        isBuyingTicket: true
      });
      buyTickets(userId, event.id, ticketsCount)
        .then(ticket => {
          if (ticket.exception) {
            this.props.userStore?.setBuyError(true);
          } else {
            this.props.userStore?.setBuyError(false);
          }
        })
        .catch(() => this.props.userStore?.setBuyError(true))
        .finally(() => {
          this.setState({
            isBuyingTicket: false,
            modalOpen: false
          });
        });
    }
  }
}