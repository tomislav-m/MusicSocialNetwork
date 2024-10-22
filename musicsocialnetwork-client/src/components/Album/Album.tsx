import React from 'react';
import { observer, inject } from 'mobx-react';
import ArtistStore from '../../stores/ArtistStore';
import { Loader, Grid, Label, Rating, Segment, Button, Divider, RatingProps } from 'semantic-ui-react';
import './Album.css';
import { Link } from 'react-router-dom';
import Tracks from './Tracks';
import UserStore from '../../stores/UserStore';
import autobind from 'autobind-decorator';
import Comments from '../Comments/Comments';
import Notification from '../../common/Notification';

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
    const albumId = this.props.match.params.id;
    const rating = this.getAlbumRating(parseInt(albumId));
    const store = this.props.artistStore;
    const userStore = this.props.userStore;
    const album = store?.album;
    const artist = store?.artist;
    const inCollection = userStore?.collection.includes(parseInt(albumId));

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
                      {
                        userStore?.userData?.id &&
                        <div>
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
                            <Notification
                              active={userStore.rateError}
                              dimmed={false}
                              negative={true}
                              title="Album rating"
                              text="Error rating album"
                            />
                          </div>
                          <div className="info-row">
                            <Button
                              icon={inCollection ? 'minus' : 'plus'}
                              size="mini"
                              color={inCollection ? 'red' : 'green'}
                              title={inCollection ? 'Remove to collection' : 'Add to collection'}
                              disabled={userStore?.isAddingToCollection}
                              loading={userStore?.isAddingToCollection}
                              onClick={() => userStore?.handleAddToCollection(album.id)}
                            />
                            <Notification
                              active={userStore.collectionError}
                              dimmed={false}
                              negative={true}
                              title="Collection"
                              text="Error adding album to collection"
                            />
                          </div>
                        </div>
                      }
                    </Grid.Column>
                    <Grid.Column width="7">
                      <Segment piled>{album?.description}</Segment>
                    </Grid.Column>
                  </Grid.Row>
                  <Divider horizontal>Comments</Divider>
                  <Grid.Row>
                    <Grid.Column width="16">
                      <Comments pageType="album" parentId={album.id} />
                    </Grid.Column>
                  </Grid.Row>
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