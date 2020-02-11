import React from 'react';
import { Modal, Button, Icon, Grid, Label } from 'semantic-ui-react';
import { EventData } from '../../models/Event';
import LinkList from '../../common/LinkList';
import { getArtist } from '../../actions/Music/MusicActions';
import { defaultArtistDataSimple } from '../../models/Artist';

interface EventInfoProps {
  event: EventData;
}

interface EventInfoState {
  artists: Array<any>;
}

export default class EventInfoModal extends React.Component<EventInfoProps, EventInfoState> {
  constructor(props: EventInfoProps) {
    super(props);

    this.state = {
      artists: []
    };
  }

  componentDidMount() {
    const event = this.props.event;
    this.getArtists([...event.headliners, ...event.supporters]);
  }

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

  private async getArtists(array: Array<number>) {
    for (const id of array) {
      const artist = await getArtist(id);
      this.setState({
        artists: [...this.state.artists, artist]
      });
    }
  }
}