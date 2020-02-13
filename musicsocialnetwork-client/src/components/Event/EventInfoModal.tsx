import React from 'react';
import { Modal, Button, Icon, Grid, Label } from 'semantic-ui-react';
import { EventData, UserEvent } from '../../models/Event';
import LinkList from '../../common/LinkList';
import { getArtist } from '../../actions/Music/MusicActions';
import { defaultArtistDataSimple } from '../../models/Artist';
import { markEvent } from '../../actions/Events/EventActions';
import autobind from 'autobind-decorator';

interface EventInfoProps {
  event: EventData;
  userId: number | undefined;
  userEvent: UserEvent;
}

interface EventInfoState {
  artists: Array<any>;
  isGoingLoading: boolean;
  isInterestedLoading: boolean;
  userEvent: UserEvent;
}

export default class EventInfoModal extends React.Component<EventInfoProps, EventInfoState> {
  constructor(props: EventInfoProps) {
    super(props);

    this.state = {
      artists: [],
      isGoingLoading: false,
      isInterestedLoading: false,
      userEvent: props.userEvent
    };
  }

  componentDidMount() {
    const event = this.props.event;
    this.getArtists([...event.headliners, ...event.supporters]);
  }

  public render() {
    const { event } = this.props;
    const { userEvent } = this.state;

    return (
      <Modal trigger={
        <Button size="mini" color="grey" icon><Icon name="angle right" size="big" title="Details" /></Button>
      } closeIcon>
        <Modal.Header>Event</Modal.Header>
        <Modal.Content>
          <Grid>
            <Grid.Row divided>
              <Grid.Column width="5">
                {
                  event.headliners.map(artistId => {
                    const artist = this.state.artists.find(x => x.id === artistId);
                    if (artist) {
                      return <img src={artist.photoUrl} alt="headliner" width="200" />;
                    }
                  })
                }{
                  event.supporters.map(artistId => {
                    const artist = this.state.artists.find(x => x.id === artistId);
                    if (artist) {
                      return <img src={artist.photoUrl} alt="supporter" width="100" />;
                    }
                  })
                }
              </Grid.Column>
              <Grid.Column width="11">
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
              </Grid.Column>
            </Grid.Row>
          </Grid>
        </Modal.Content>
        <Modal.Actions>
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
}