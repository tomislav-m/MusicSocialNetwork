import { ArtistData, defaultArtistData } from '../models/Artist';
import { artistData } from '../data/ArtistDataMock';
import autobind from 'autobind-decorator';
import { action, observable } from 'mobx';
import { AlbumData, defaultAlbumData } from '../models/Album';
import { albumData } from '../data/AlbumDataMock';
import _ from 'lodash';
import { getArtist, getAlbum } from '../actions/Music/MusicActions';

export default class ArtistStore {
  artists: Array<ArtistData> = artistData;
  @observable artist: ArtistData = defaultArtistData;

  albumsAll: Array<AlbumData> = albumData;
  @observable albums: Array<AlbumData> = [];

  @observable album: AlbumData = defaultAlbumData;

  @autobind
  @action
  setArtist(id: number) {
    getArtist(id)
      .then((result: ArtistData) => {
        this.artist = result;
      });
  }

  @autobind
  @action
  setAlbums(artistId: number) {
    this.albums = [];
    this.albums = _.sortBy(this.albumsAll.filter(x => x.artistId === artistId), x => x.yearReleased);
  }

  @autobind
  @action
  setAlbum(id: number) {
    getAlbum(id)
      .then((result: AlbumData) => {
        this.album = result;

        if (this.artist.id !== result.artistId) {
          this.setArtist(result.artistId);
        }
      });
  }
}