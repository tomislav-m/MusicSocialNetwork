import { ArtistData } from '../models/Artist';
import { artistData } from '../data/ArtistDataMock';
import autobind from 'autobind-decorator';
import { action, observable } from 'mobx';
import { AlbumData } from '../models/Album';
import { albumData } from '../data/AlbumDataMock';
import _ from 'lodash';

export default class ArtistStore {
  artists: Array<ArtistData> = artistData;
  @observable artist: ArtistData | undefined = undefined;

  albumsAll: Array<AlbumData> = albumData;
  @observable albums: Array<AlbumData> | undefined = undefined;

  @observable album: AlbumData | undefined = undefined;

  @autobind
  @action
  setArtist(id: number) {
    this.artist = undefined;
    this.artist = this.artists.filter(x => x.Id === id)[0];
  }

  @autobind
  @action
  setAlbums(artistId: number) {
    this.albums = undefined;
    this.albums = _.sortBy(this.albumsAll.filter(x => x.ArtistId === artistId), x => x.YearReleased);
  }

  @autobind
  @action
  setAlbum(id: number) {
    this.album = undefined;
    this.album = this.albums?.filter(x => x.Id === id)[0];
  }
}