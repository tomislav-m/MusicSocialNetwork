import React from 'react';
import { observer, inject } from 'mobx-react';
import ArtistStore from '../../stores/ArtistStore';
import { Loader, Grid, Label, Icon, Segment, Table, Tab } from 'semantic-ui-react';
import './Artist.css';
import { Link } from 'react-router-dom';
import EventList from '../Event/EventList';

interface ArtistProps {
  artistStore?: ArtistStore;
  match: any;
}

@inject('artistStore')
@observer
export default class Artist extends React.Component<ArtistProps> {
  constructor(props: ArtistProps) {
    super(props);

    const id = parseInt(props.match.params.id);
    this.updateArtist(props.artistStore, id);
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
    store?.setAlbums(id);

    if (store && store.artist && store.albums) {
      store.artist.Genres = Array.from(new Set(store.albums.map(x => x.Genre)));
      store.artist.Styles = Array.from(new Set(store.albums.map(x => x.Style)));
    }
  }

   private panes: any = [
    { menuItem: 'Albums', render: () => <Tab.Pane>{this.renderAlbums()}</Tab.Pane> },
    { menuItem: 'Events', render: () => <Tab.Pane><EventList events={this.props.artistStore?.artist?.Events} /></Tab.Pane> }
  ];

  public render() {
    const artist = this.props.artistStore?.artist;

    const year = artist?.YearFormed !== 0 ? artist?.YearFormed : artist.YearBorn;

    return (
      <div>
        {artist === undefined ?
          <Loader active /> :
          <Grid>
            <Grid.Row divided>
              <Grid.Column width="5" >
                <img src={artist?.PhotoUrl} alt="cover" width={320} />
                <br />
                <a href={`//${artist.FacebookUrl}`} target="_blank" rel="noopener noreferrer" >
                  <Icon name="facebook f" link={true} size="large" />
                </a>
                <a href={`//${artist.WebsiteUrl}`} target="_blank" rel="noopener noreferrer" >
                  <Icon name="globe" link={true} size="large" color="black" />
                </a>
                <Segment>{artist.Bio}</Segment>
              </Grid.Column>
              <Grid.Column width="11" >
                <h2>{artist.Name}</h2>
                <div className="info-row">
                  <Label className="info-label">Formed</Label><span className="info">{year}, {artist.Country}</span>
                </div>
                <div className="info-row">
                  <Label className="info-label">Styles</Label>
                  <span className="info">
                    {artist.Styles?.map(x => <Label key={x} tag color="red" size="tiny">{x}</Label>)}
                  </span>
                </div>
                <div className="info-row">
                  <Label className="info-label">Genres</Label>
                  <span className="info">
                    {artist.Genres?.map(x => <Label key={x} tag color="blue" size="tiny">{x}</Label>)}
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
            <Table.Row key={album.Id}>
              <Table.Cell width={1}>
                <Link to={`/Album/${album.Id}`}>
                  <img src={album.CoverArtUrl} width={40} alt="Cover art" />
                </Link>
              </Table.Cell>
              <Table.Cell width={9} verticalAlign="middle">
                <Link to={`/Album/${album.Id}`}>
                  {<strong>{album.Name}</strong>} {`(${album.YearReleased})`}
                </Link>
              </Table.Cell>
              <Table.Cell width={3}>{album.RatingData.RatingCount}</Table.Cell>
              <Table.Cell width={3}>{album.RatingData.AverageRating} / 10</Table.Cell>
            </Table.Row>
          )
          }
        </Table.Body>
      </Table>
    );
  }
}