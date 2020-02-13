import React from 'react';
import { observer, inject } from 'mobx-react';
import ArtistStore from '../../stores/ArtistStore';
import { Loader, Grid, Label, Icon, Segment, Table, Tab } from 'semantic-ui-react';
import './Artist.css';
import { Link } from 'react-router-dom';
import EventList from '../Event/EventList';
import UserStore from '../../stores/UserStore';
import { arrayToList } from '../../common/helpers';

interface ArtistProps {
  artistStore?: ArtistStore;
  userStore?: UserStore;
  match: any;
}

@inject('artistStore')
@inject('userStore')
@observer
export default class Artist extends React.Component<ArtistProps> {
  constructor(props: ArtistProps) {
    super(props);

    const id = parseInt(props.match.params.id);
    if (id !== props.artistStore?.artist.id) {
      this.updateArtist(props.artistStore, id);
    }
  }

  componentDidUpdate(prevProps: ArtistProps) {
    const id = parseInt(this.props.match.params.id);
    const prevId = parseInt(prevProps.match.params.id);

    if (prevId !== id) {
      this.updateArtist(this.props.artistStore, id);
    }
  }

  private updateArtist(store: ArtistStore | undefined, id: number) {
    store?.setArtist(id);

    if (store && store.artist && store.albums) {
      store.artist.genres = Array.from(new Set(store.albums.map(x => x.genre)));
      store.artist.styles = Array.from(new Set(store.albums.map(x => x.style)));
    }
  }

  private panes: any = [
    { menuItem: 'Albums', render: () => <Tab.Pane>{this.renderAlbums()}</Tab.Pane> },
    { menuItem: 'Events', render: () => <Tab.Pane><EventList store={this.props.artistStore} artistId={this.props.artistStore?.artist.id || 0} simpleArtistsDict={this.props.artistStore?.simpleArtistsDict} userId={this.props.userStore?.userData?.id} userEvents= {this.props.userStore?.userEvents || []} /></Tab.Pane> }
  ];

  public render() {
    const artist = this.props.artistStore?.artist;
    const genres = this.props.artistStore?.genres;
    const styles = this.props.artistStore?.styles;

    const year = artist?.yearFormed !== 0 ? artist?.yearFormed : artist.yearBorn;

    return (
      <div>
        {artist === undefined || this.props.artistStore?.isLoading ?
          <Loader active /> :
          <Grid>
            <Grid.Row divided>
              <Grid.Column width="5" >
                <img src={artist?.photoUrl} alt="cover" width={320} />
                <br />
                <a href={`//${artist.facebookUrl}`} target="_blank" rel="noopener noreferrer" >
                  <Icon name="facebook f" link={true} size="large" />
                </a>
                <a href={`//${artist.websiteUrl}`} target="_blank" rel="noopener noreferrer" >
                  <Icon name="globe" link={true} size="large" color="black" />
                </a>
                <Segment>{artist.bio}</Segment>
              </Grid.Column>
              <Grid.Column width="11" >
                <h2>{artist.name}</h2>
                <div className="info-row">
                  <Label className="info-label">Formed</Label><span className="info">{year}, {artist.country}</span>
                </div>
                <div className="info-row">
                  <Label className="info-label">Styles</Label>
                  <span className="info">
                    {arrayToList(styles || [])}
                  </span>
                </div>
                <div className="info-row">
                  <Label className="info-label">Genres</Label>
                  <span className="info">
                    {arrayToList(genres || [])}
                  </span>
                </div>
                <Tab panes={this.panes} />
              </Grid.Column>
            </Grid.Row>
          </Grid>
        }
      </div>
    );
  }

  private renderAlbums() {
    const albums = this.props.artistStore?.albums;

    return (
      <Table striped compact>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell></Table.HeaderCell>
            <Table.HeaderCell>Album</Table.HeaderCell>
            <Table.HeaderCell>No. of ratings</Table.HeaderCell>
            <Table.HeaderCell>Rating</Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {albums && albums.map(album =>
            <Table.Row key={album.id}>
              <Table.Cell width={1}>
                <Link to={`/Album/${album.id}`}>
                  <img src={album.coverArtUrl} width={40} alt="Cover art" />
                </Link>
              </Table.Cell>
              <Table.Cell width={9} verticalAlign="middle">
                <Link to={`/Album/${album.id}`}>
                  {<strong>{album.name}</strong>} {`(${album.yearReleased})`}
                </Link>
              </Table.Cell>
              <Table.Cell width={3}>{album.ratingData?.ratingCount || 0}</Table.Cell>
              <Table.Cell width={3}>{album.ratingData?.averageRating || 0} / 10</Table.Cell>
            </Table.Row>
          )
          }
        </Table.Body>
      </Table>
    );
  }
}