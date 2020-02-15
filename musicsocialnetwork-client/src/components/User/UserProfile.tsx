import React from 'react';
import { inject, observer } from 'mobx-react';
import UserStore from '../../stores/UserStore';
import { Grid, Table, Icon, Pagination, Divider } from 'semantic-ui-react';
import ArtistStore from '../../stores/ArtistStore';
import { Link, Redirect } from 'react-router-dom';
import autobind from 'autobind-decorator';
import { getAlbum, getArtist } from '../../actions/Music/MusicActions';
import Recommendations from './Recommendations';

interface UserProps {
  userStore?: UserStore;
  artistStore?: ArtistStore;
  pageSize?: number;
}

interface UserState {
  albumRatings?: Array<any>;
  ratingsPage: number;
  eventsPage: number;
  pageSize: number;
}

@inject('userStore')
@inject('artistStore')
@observer
export default class UserProfile extends React.Component<UserProps, UserState> {
  constructor(props: UserProps) {
    super(props);

    this.state = {
      albumRatings: [],
      ratingsPage: 1,
      eventsPage: 1,
      pageSize: props.pageSize || 5
    };
  }

  componentDidMount() {
    this.getAlbumsByRating();
  }

  public render() {
    const userStore = this.props.userStore;
    const events = userStore?.events || [];
    const albumRatings = this.props.userStore?.albumRatings || [];
    const pageSize = this.state.pageSize;

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
                        <Table.Cell>{event.date}</Table.Cell>
                        <Table.Cell>{event.venue}</Table.Cell>
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
          <Grid.Row>
            <Grid.Column width={10}>
              <Recommendations userid={userStore?.userData?.id || 0} />
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

    this.props.userStore?.userData?.ratings.forEach(async rating => {
      getAlbum(rating.albumId).then(album => {
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

}