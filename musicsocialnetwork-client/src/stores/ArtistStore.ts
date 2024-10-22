import { ArtistData, defaultArtistData, ExtendedArtistData } from '../models/Artist';
import { artistData } from '../data/ArtistDataMock';
import autobind from 'autobind-decorator';
import { action, observable, computed } from 'mobx';
import { AlbumData, defaultAlbumData, AlbumRatingData } from '../models/Album';
import { albumData } from '../data/AlbumDataMock';
import { getArtist, getAlbum, getAverageRating, getArtistNames } from '../actions/Music/MusicActions';
import { EventData } from '../models/Event';
import { getEventsByArtist } from '../actions/Events/EventActions';

export default class ArtistStore {
  artists: Array<ArtistData> = artistData;
  @observable artist: ArtistData = defaultArtistData;

  albumsAll: Array<AlbumData> = albumData;
  @observable albums: Array<AlbumData> = [];

  @observable album: AlbumData = defaultAlbumData;

  @observable events: Array<EventData> = [];
  @observable simpleArtistsDict: { [id: number]: string } = {};

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

        this.setEvents(result.id);
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
      .then(async (result: AlbumData) => {
        this.album = result;
        const avgRating = await getAverageRating(id);
        this.album.ratingData = avgRating;

        if (this.artist.id !== result.artistId) {
          this.setArtist(result.artistId);
        }
      })
      .finally(() => this.isLoading = false);
  }

  @autobind
  @action
  setEvents(artistId: number) {
    const ids: Array<number> = [artistId];
    this.events.length = 0;
    getEventsByArtist(artistId)
      .then((events: Array<EventData>) => {
        events.forEach(event => ids.push(...event.headliners.concat(event.supporters)));
        this.events.length = 0;
        this.events.push(...events);
      })
      .finally(async () => {
        this.simpleArtistsDict = {...await getArtistNames(Array.from(new Set(ids)))};
      });
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

  @computed
  get sortedAlbums() {
    return this.albums.slice().sort((x, y) => x.yearReleased - y.yearReleased);
  }
}