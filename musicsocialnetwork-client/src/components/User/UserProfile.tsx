import React from 'react';
import { inject, observer } from 'mobx-react';
import UserStore from '../../stores/UserStore';
import { Grid, Table, Icon, Pagination, Divider } from 'semantic-ui-react';
import ArtistStore from '../../stores/ArtistStore';
import { Link, Redirect } from 'react-router-dom';
import autobind from 'autobind-decorator';
import { EventData, UserEvent } from '../../models/Event';
import EventInfoModal from '../Event/EventInfoModal';
import LinkList from '../../common/LinkList';
import { getAlbum, getArtist } from '../../actions/Music/MusicActions';

interface UserProps {
  userStore?: UserStore;
  artistStore?: ArtistStore;
  pageSize?: number;
}

interface UserState {
  albumRatings?: Array<any>;
  events?: Array<EventData>;
  ratingsPage: number;
  eventsPage: number;
  pageSize: number;
  userEvents: Array<UserEvent>;
}

@inject('userStore')
@inject('artistStore')
@observer
export default class UserProfile extends React.Component<UserProps, UserState> {
  constructor(props: UserProps) {
    super(props);

    const events = props.userStore?.userData?.events;

    this.state = {
      albumRatings: [],
      ratingsPage: 1,
      eventsPage: 1,
      pageSize: props.pageSize || 5,
      events,
      userEvents: props.userStore?.userEvents || []
    };
  }

  componentDidMount() {
    this.getAlbumsByRating();
  }

  public render() {
    const userStore = this.props.userStore;
    const albumRatings = this.props.userStore?.albumRatings || [];
    const events = this.state.events || [];
    const pageSize = this.state.pageSize;
    const dict = this.props.artistStore?.simpleArtistsDict || {};

    return (
      <div>
        {
          !this.props.userStore?.isLoggedIn && <Redirect to="/" />
        }
        <Grid>
          <Grid.Row>
            <h2>{this.props.userStore?.userData?.username}</h2>
          </Grid.Row>
          <Grid.Row divided>
            <Grid.Column width="9">
              <Table striped>
                <Table.Header>
                  <Table.Row>
                    <Table.HeaderCell colSpan="4">Album ratings</Table.HeaderCell>
                  </Table.Row>
                </Table.Header>
                <Table.Body>
                  {
                    albumRatings.slice((this.state.ratingsPage - 1) * pageSize, this.state.ratingsPage * pageSize).map(rating =>
                      <Table.Row key={rating.albumId}>
                        <Table.Cell><img width="50px" src={rating.coverUrl} alt="cover" /></Table.Cell>
                        <Table.Cell>{new Date(rating.createdAt).toLocaleDateString('hr-HR')}</Table.Cell>
                        <Table.Cell><Icon name="star" color="yellow" />x{rating.rating}</Table.Cell>
                        <Table.Cell>
                          <strong><Link to={`/Artist/${rating.artistId}`}>{rating.artist}</Link></strong> - <Link to={`/Album/${rating.albumId}`}>{rating.album}</Link> ({rating.yearReleased})
                      </Table.Cell>
                      </Table.Row>
                    )
                  }
                </Table.Body>
              </Table>
              {
                albumRatings.length > pageSize &&
                <Pagination activePage={this.state.ratingsPage} totalPages={albumRatings.length / pageSize} onPageChange={this.handleRatingsPageChange} />
              }
            </Grid.Column>
            <Grid.Column width="7">
              <Table striped singleLine>
                <Table.Header>
                  <Table.Row>
                    <Table.HeaderCell colSpan="4">My Events</Table.HeaderCell>
                  </Table.Row>
                </Table.Header>
                <Table.Body>
                  {
                    events.map(event =>
                      <Table.Row key={event.id}>
                        <Table.Cell>{event.date.toLocaleDateString('hr-HR')}</Table.Cell>
                        <Table.Cell>{event.venue}</Table.Cell>
                        <Table.Cell><LinkList artists={event.headliners.concat(event.supporters).map(x => { return { id: event.id, name: dict[x] }; })} /></Table.Cell>
                        <Table.Cell><EventInfoModal event={event} userId={userStore?.userData?.id} userEvent={this.state.userEvents.filter(x => x.eventId === event.id)[0]} /></Table.Cell>
                      </Table.Row>
                    )
                  }
                </Table.Body>
              </Table>
              {
                events.length > pageSize &&
                <Pagination activePage={this.state.eventsPage} totalPages={events.length / pageSize} onPageChange={this.handleEventsPageChange} />
              }
            </Grid.Column>
          </Grid.Row>
          <Divider horizontal>Comments</Divider>
          <Grid.Row>
          </Grid.Row>
        </Grid>
      </div>
    );
  }

  @autobind
  private handleRatingsPageChange(event: any, { activePage }: any) {
    this.setState({
      ratingsPage: activePage
    });
  }

  @autobind
  private handleEventsPageChange(event: any, { activePage }: any) {
    this.setState({
      eventsPage: activePage
    });
  }

  private getAlbumsByRating() {
    const ratings = this.props.userStore?.albumRatings || [];
    ratings.length = 0;

    this.props.userStore?.userData?.ratings.map(async rating => {
      await getAlbum(rating.albumId).then(album => {
        return {
          album: album?.name,
          albumId: rating.albumId,
          artistId: album.artistId,
          artist: '',
          rating: rating.rating,
          yearReleased: album?.yearReleased,
          coverUrl: album?.coverArtUrl,
          createdAt: rating.createdAt
        };
      })
        .then(async data => {
          const artist = await getArtist(data.artistId);
          data.artist = artist.name;
          this.props.userStore?.albumRatings.push(data);
        });
    });
  }

  private sortRatings = (a: any, b: any): number => {
    if (a.createdAt < b.createdAt) {
      return 1;
    }

    if (a.createdAt > b.createdAt) {
      return -1;
    }

    return a.album > b.album ? 1 : -1;
  }
}