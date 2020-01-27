import { ArtistData, defaultArtistData, ExtendedArtistData } from '../models/Artist';
import { artistData } from '../data/ArtistDataMock';
import autobind from 'autobind-decorator';
import { action, observable, computed } from 'mobx';
import { AlbumData, defaultAlbumData, AlbumRatingData } from '../models/Album';
import { albumData } from '../data/AlbumDataMock';
import { getArtist, getAlbum, getAverageRating } from '../actions/Music/MusicActions';

export default class ArtistStore {
  artists: Array<ArtistData> = artistData;
  @observable artist: ArtistData = defaultArtistData;

  albumsAll: Array<AlbumData> = albumData;
  @observable albums: Array<AlbumData> = [];

  @observable album: AlbumData = defaultAlbumData;

  @observable isLoading: boolean = false;

  @autobind
  @action
  setArtist(id: number) {
    this.isLoading = true;
    getArtist(id)
      .then((result: ExtendedArtistData) => {
        this.artist = result;

        this.setAlbums(result.albums)
          .then(() => this.isLoading = false);
      });
  }

  @autobind
  @action
  async setAlbums(albums: Array<AlbumData>) {
    this.albums.length = 0;
    for (const album of albums) {
      await getAverageRating(album.id)
        .then((avgRating: AlbumRatingData) => {
          album.ratingData = avgRating;
          this.albums.push(album);
        });
    }
  }

  @autobind
  @action
  setAlbum(id: number) {
    this.isLoading = true;
    getAlbum(id)
      .then((result: AlbumData) => {
        this.album = result;

        if (this.artist.id !== result.artistId) {
          this.setArtist(result.artistId);
        }
      })
      .then(() => this.isLoading = false);
  }

  @computed
  get genres(): Array<string> {
    const genres = this.albums.map(x => x.genre);
    return Array.from(new Set(genres));
  }

  @computed
  get styles(): Array<string> {
    return Array.from(new Set(this.albums.map(x => x.style)));
  }
}