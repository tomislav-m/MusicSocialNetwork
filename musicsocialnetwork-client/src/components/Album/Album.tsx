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
    if (props.artistStore?.album.id !== id) {
      props.artistStore?.setAlbum(id);
    }
  }

  public render() {
    const rating = this.getAlbumRating(parseInt(this.props.match.params.id));
    const store = this.props.artistStore;
    const userStore = this.props.userStore;
    const album = store?.album;
    const artist = store?.artist;

    return (
      <div>
        {album === undefined || this.props.artistStore?.isLoading ?
          <Loader active /> :
          <Grid relaxed>
            <Grid.Row divided>
              <Grid.Column width="5">
                <img src={album?.coverArtUrl} alt="cover" width={320} />
                <Tracks tracks={album.tracks} />
              </Grid.Column>
              <Grid.Column width="11">
                <Grid divided>
                  <Grid.Row>
                    <Grid.Column width="9">
                      <h2>{album.name}</h2>
                      <div className="info-row">
                        <Label className="info-label">Artist</Label>
                        <Link to={`/Artist/${artist?.id}`} className="link-artist">
                          <span className="info">{artist?.name}</span>
                        </Link>
                      </div>
                      <div className="info-row">
                        <Label className="info-label">Year</Label><span className="info">{album?.yearReleased}</span>
                      </div>
                      <div className="info-row">
                        <Label className="info-label">Style</Label><span className="info">{album?.style}</span>
                      </div>
                      <div className="info-row">
                        <Label className="info-label">Genre</Label><span className="info">{album?.genre}</span>
                      </div>
                      <div className="info-row">
                        <Label className="info-label">Format</Label><span className="info">{album?.format}</span>
                      </div>
                      <div className="info-row">
                        <Label className="info-label">Rating</Label>
                        <span className="info">{album.ratingData?.averageRating || 0} / 10</span>
                      </div>
                      <div className="info-row">
                        <Label className="info-label">Ratings</Label>
                        <span className="info">{album.ratingData?.ratingCount || 0}</span>
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
                      <Segment piled>{album?.description}</Segment>
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
    const ratings = this.props.userStore?.userData?.ratings;
    const rating = ratings?.find(x => x.albumId === id);

    return rating ? rating.rating : 0;
  }

  @autobind
  private handleRateAlbum(event: any, data: RatingProps) {
    const albumId = this.props.artistStore?.album?.id;
    if (albumId) {
      this.props.userStore?.rateAlbum(albumId, data.rating as number);
    }
  }
}