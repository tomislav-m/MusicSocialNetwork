import React from 'react';
import { Grid, Table } from 'semantic-ui-react';
import autobind from 'autobind-decorator';
import { getPopularAlbums, getSimpleAlbums } from '../../actions/Music/MusicActions';
import { AlbumDataSimple } from '../../models/Album';
import { Link } from 'react-router-dom';

interface PopularAlbumsProps { }

interface PopularAlbumsState {
  todayAlbums: Array<AlbumDataSimple>;
  weekAlbums: Array<AlbumDataSimple>;
  monthAlbums: Array<AlbumDataSimple>;
  isLoading: boolean;
}

export default class PopularAlbums extends React.Component<PopularAlbumsProps, PopularAlbumsState> {

  constructor(props: PopularAlbumsProps) {
    super(props);

    this.state = {
      todayAlbums: [],
      weekAlbums: [],
      monthAlbums: [],
      isLoading: true
    };
  }

  componentDidMount() {
    this.handleGetPopularAlbums();
  }

  render() {
    const { todayAlbums, weekAlbums, monthAlbums } = this.state;

    return (
      <div>
        <h2>Most popular albums</h2>
        <Grid>
          <Grid.Row>
            <Grid.Column width="5">
              <Table striped>
                <Table.Header>
                  <Table.Row>
                    <Table.HeaderCell>Today</Table.HeaderCell>
                    <Table.HeaderCell></Table.HeaderCell>
                    <Table.HeaderCell></Table.HeaderCell>
                  </Table.Row>
                </Table.Header>
                <Table.Body>
                  {
                    todayAlbums.map(album =>
                      <Table.Row key={album.id}>
                        <Table.Cell><img src={album.coverArtUrl} alt="cover" width="50px" /></Table.Cell>
                        <Table.Cell><Link to={`/Artist/${album.artist.id}`}>{album.artist.name}</Link></Table.Cell>
                        <Table.Cell><Link to={`/Album/${album.id}`}>{album.name}</Link></Table.Cell>
                      </Table.Row>
                    )
                  }
                </Table.Body>
              </Table>
            </Grid.Column>
            <Grid.Column width="5">
              <Table striped>
                <Table.Header>
                  <Table.Row>
                    <Table.HeaderCell>Week</Table.HeaderCell>
                    <Table.HeaderCell></Table.HeaderCell>
                    <Table.HeaderCell></Table.HeaderCell>
                  </Table.Row>
                </Table.Header>
                <Table.Body>
                  {
                    weekAlbums.map(album =>
                      <Table.Row key={album.id}>
                        <Table.Cell><img src={album.coverArtUrl} alt="cover" width="50px" /></Table.Cell>
                        <Table.Cell><Link to={`/Artist/${album.artist.id}`}>{album.artist.name}</Link></Table.Cell>
                        <Table.Cell><Link to={`/Album/${album.id}`}>{album.name}</Link></Table.Cell>
                      </Table.Row>
                    )
                  }
                </Table.Body>
              </Table>
            </Grid.Column>
            <Grid.Column width="5">
              <Table striped>
                <Table.Header>
                  <Table.Row>
                    <Table.HeaderCell>Month</Table.HeaderCell>
                    <Table.HeaderCell></Table.HeaderCell>
                    <Table.HeaderCell></Table.HeaderCell>
                  </Table.Row>
                </Table.Header>
                <Table.Body>
                  {
                    monthAlbums.map(album =>
                      <Table.Row key={album.id}>
                        <Table.Cell><img src={album.coverArtUrl} alt="cover" width="50px" /></Table.Cell>
                        <Table.Cell><Link to={`/Artist/${album.artist.id}`}>{album.artist.name}</Link></Table.Cell>
                        <Table.Cell><Link to={`/Album/${album.id}`}>{album.name}</Link></Table.Cell>
                      </Table.Row>
                    )
                  }
                </Table.Body>
              </Table>
            </Grid.Column>
          </Grid.Row>
        </Grid>
      </div>
    );
  }

  @autobind
  private handleGetPopularAlbums() {
    getPopularAlbums()
      .then(result => {
        const todayAlbumIds = result.todayAlbums;
        const weekAlbumIds = result.weekAlbums;
        const monthAlbumIds = result.monthAlbums;

        getSimpleAlbums([...todayAlbumIds, ...weekAlbumIds, ...monthAlbumIds])
          .then((simpleAlbums: Array<AlbumDataSimple>) => {
            const todayAlbums: Array<any> = [];
            const weekAlbums: Array<any> = [];
            const monthAlbums: Array<any> = [];

            todayAlbumIds.forEach((id: number) => {
              todayAlbums.push(simpleAlbums.filter(x => x.id === id)[0]);
            });
            weekAlbumIds.forEach((id: number) => {
              weekAlbums.push(simpleAlbums.filter(x => x.id === id)[0]);
            });
            monthAlbumIds.forEach((id: number) => {
              monthAlbums.push(simpleAlbums.filter(x => x.id === id)[0]);
            });

            this.setState({
              todayAlbums,
              weekAlbums,
              monthAlbums
            });
          })
          .finally(() => this.setState({ isLoading: false }));
      });
  }
}