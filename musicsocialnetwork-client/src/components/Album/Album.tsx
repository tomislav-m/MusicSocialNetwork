import React from 'react';
import { observer, inject } from 'mobx-react';
import ArtistStore from '../../stores/ArtistStore';
import { Loader, Grid, Label, Rating, Segment, Button, Divider, RatingProps } from 'semantic-ui-react';
import './Album.css';
import { Link } from 'react-router-dom';
import Tracks from './Tracks';
import UserStore from '../../stores/UserStore';
import autobind from 'autobind-decorator';

interface AlbumProps {
  artistStore?: ArtistStore;
  userStore?: UserStore;
  match: any;
}

@inject('artistStore')
@inject('userStore')
@observer
export default class Album extends React.Component<AlbumProps> {
  constructor(props: AlbumProps) {
    super(props);

    const id = parseInt(props.match.params.id);
    props.artistStore?.setAlbum(id);
  }

  public render() {
    const rating = this.getAlbumRating(parseInt(this.props.match.params.id));
    const store = this.props.artistStore;
    const userStore = this.props.userStore;
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
              <Grid.Column width="11">
                <Grid divided>
                  <Grid.Row>
                    <Grid.Column width="9">
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
                      <Divider horizontal />
                      <div className="info-row">
                        <Label className="info-label">Your rating</Label>
                        <span className="info">
                          <Rating
                            icon="star"
                            maxRating={10}
                            rating={rating}
                            size="small"
                            clearable
                            disabled={!userStore?.isLoggedIn}
                            onRate={this.handleRateAlbum} />
                        </span>
                      </div>
                      <div className="info-row">
                        <Button icon="plus" size="mini" color="green" title="Add to collection" />
                      </div>
                    </Grid.Column>
                    <Grid.Column width="7">
                      <Segment piled>{album?.Description}</Segment>
                    </Grid.Column>
                  </Grid.Row>
                  <Divider horizontal>Comments</Divider>
                  <Grid.Row></Grid.Row>
                </Grid>
              </Grid.Column>
            </Grid.Row>
          </Grid>
        }
      </div>
    );
  }

  private getAlbumRating(id: number) {
    const ratings = this.props.userStore?.userData?.Ratings;
    const rating = ratings?.find(x => x.AlbumId === id);

    return rating ? rating.Rating : 0;
  }

  @autobind
  private handleRateAlbum(event: any, data: RatingProps) {
    const albumId = this.props.artistStore?.album?.Id;
    if (albumId) {
      this.props.userStore?.rateAlbum(albumId, data.rating as number);
    }
  }
}