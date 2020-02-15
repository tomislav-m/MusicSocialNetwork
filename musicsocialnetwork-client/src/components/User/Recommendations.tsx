import React from 'react';
import { Tab, Table, Loader } from 'semantic-ui-react';
import { AlbumDataSimple } from '../../models/Album';
import { ArtistDataSimple } from '../../models/Artist';
import { Link } from 'react-router-dom';
import { getRecommendations, getSimpleAlbums, getArtistNames } from '../../actions/Music/MusicActions';
import autobind from 'autobind-decorator';

interface RecommendationProps {
  userid: number;
}

interface RecommendationState {
  artists: Array<ArtistDataSimple>;
  albums: Array<AlbumDataSimple>;
  isLoading: boolean;
}

export default class Recommendations extends React.Component<RecommendationProps, RecommendationState> {
  constructor(props: RecommendationProps) {
    super(props);

    this.state = {
      albums: [],
      artists: [],
      isLoading: true
    };
  }

  componentDidMount() {
    getRecommendations(this.props.userid)
      .then((recommendation) => {
        if (!recommendation || recommendation.exception) {
          return;
        }
        getSimpleAlbums(recommendation.albumIds)
          .then(async (albums: Array<AlbumDataSimple>) => {
            const sortedArtistIds = this.countAndSortArtists(albums.map(x => x.artist.id));
            const simpleArtists = await getArtistNames(sortedArtistIds);
            albums.forEach(album => {
              album.artist.name = simpleArtists[album.artist.id];
            });

            const artists: Array<ArtistDataSimple> = [];
            sortedArtistIds.forEach(id => {
              artists.push({ id, name: simpleArtists[id] });
            });
            this.setState({
              albums,
              artists,
              isLoading: false
            });
          });
      });
  }

  private panes: any = [
    { menuItem: 'Albums', render: this.renderAlbumsTab },
    { menuItem: 'Artists', render: this.renderArtistsTab }
  ];

  public render() {
    return (
      <div>
        <h2>Recommendations</h2>
        {
          this.state.isLoading ?
            <Loader active={this.state.isLoading} /> :
            <Tab panes={this.panes} />
        }
      </div>
    );
  }

  @autobind
  private renderAlbumsTab() {
    const { albums } = this.state;

    return (
      <Table striped>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell></Table.HeaderCell>
            <Table.HeaderCell>Artist</Table.HeaderCell>
            <Table.HeaderCell>Album</Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {
            albums && albums.slice(0, 10).map(album =>
              <Table.Row key={album.id}>
                <Table.Cell width={2}>
                  <Link to={`/Album/${album.id}`}>
                    <img src={album.coverArtUrl} width={40} alt="Cover art" />
                  </Link>
                </Table.Cell>
                <Table.Cell width={7} verticalAlign="middle">
                  <Link to={`/Artist/${album.artist.id}`}>
                    {<strong>{album.artist.name}</strong>}
                  </Link>
                </Table.Cell>
                <Table.Cell width={7} verticalAlign="middle">
                  <Link to={`/Album/${album.id}`}>
                    {<strong>{album.name}</strong>}
                  </Link>
                </Table.Cell>
              </Table.Row>
            )
          }
        </Table.Body>
      </Table>
    );
  }

  @autobind
  private renderArtistsTab() {
    const artists = this.state.artists;

    return (
      <Table striped>
        <Table.Body>
          {
            artists && artists.slice(0, 10).map(artist =>
              <Table.Row key={artist.id}>
                <Table.Cell width={2}></Table.Cell>
                <Table.Cell width={7} verticalAlign="middle">
                  <Link to={`/Artist/${artist.id}`}>
                    {<strong>{artist.name}</strong>}
                  </Link>
                </Table.Cell>
                <Table.Cell width={7} verticalAlign="middle"></Table.Cell>
              </Table.Row>
            )
          }
        </Table.Body>
      </Table>
    );
  }

  private countAndSortArtists(artistIds: Array<number>): Array<number> {
    const artists = new Map<number, number>();
    artistIds.forEach(x => {
      if (artists.has(x)) {
        artists.set(x, (artists.get(x) || 0) + 1);
      } else {
        artists.set(x, 1);
      }
    });
    let array = Array.from(artists);
    array = array.sort((x, y) => y[1] - x[1]);
    return array.map(x => x[0]);
  }
}