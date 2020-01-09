import React from 'react';
import { observer, inject } from 'mobx-react';
import ArtistStore from '../../stores/ArtistStore';
import { Loader, Grid, Label, Rating, Segment, Button } from 'semantic-ui-react';
import './Album.css';
import { Link } from 'react-router-dom';
import Tracks from './Tracks';

interface AlbumProps {
  artistStore?: ArtistStore;
  match: any;
}

@inject('artistStore')
@observer
export default class Album extends React.Component<AlbumProps> {
  constructor(props: AlbumProps) {
    super(props);

    const id = parseInt(props.match.params.id);
    props.artistStore?.setAlbum(id);
  }

  public render() {
    const store = this.props.artistStore;
    const album = store?.album;
    const artist = store?.artists.find(x => x.Id === album?.ArtistId);
    return (
      <div>
        {album === undefined ?
          <Loader active /> :
          <Grid relaxed>
            <Grid.Row divided>
              <Grid.Column width="5">
                <img src={album?.CoverArtUrl} alt="cover" width={320} />
                <Tracks tracks={album.Tracks} />
              </Grid.Column>
              <Grid.Column width="6">
                <h2>{album.Name}</h2>
                <div className="info-row">
                  <Label className="info-label">Artist</Label>
                  <Link to={`/Artist/${artist?.Id}`} className="link-artist">
                    <span className="info">{artist?.Name}</span>
                  </Link>
                </div>
                <div className="info-row">
                  <Label className="info-label">Year</Label><span className="info">{album?.YearReleased}</span>
                </div>
                <div className="info-row">
                  <Label className="info-label">Style</Label><span className="info">{album?.Style}</span>
                </div>
                <div className="info-row">
                  <Label className="info-label">Genre</Label><span className="info">{album?.Genre}</span>
                </div>
                <div className="info-row">
                  <Label className="info-label">Format</Label><span className="info">{album?.Format}</span>
                </div>
                <div className="info-row">
                  <Label className="info-label">Rating</Label>
                  <span className="info">{album?.RatingData.AverageRating} / 10</span>
                </div>
                <div className="info-row">
                  <Label className="info-label">Ratings</Label>
                  <span className="info">{album?.RatingData.RatingCount}</span>
                </div>
                <div className="info-row">
                  <Label className="info-label">Your rating</Label>
                  <span className="info">
                    <Rating icon="star" maxRating={10} size="small" clearable />
                  </span>
                </div>
                <div className="info-row">
                  <Button icon="plus" size="mini" color="green" title="Add to collection" />
                </div>
              </Grid.Column>
              <Grid.Column width="5">
                <Segment piled>{album?.Description}</Segment>
              </Grid.Column>
            </Grid.Row>
          </Grid>
        }
      </div>
    );
  }
}