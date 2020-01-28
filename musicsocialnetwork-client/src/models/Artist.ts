import { EventData } from './Event';
import { AlbumData } from './Album';

export interface ArtistSearchData {
  id: number;
  name: string;
  photoUrl: string;
}

export interface ArtistData {
  id: number;
  name: string;
  photoUrl: string;
  websiteUrl: string;
  facebookUrl: string;
  bio: string;
  yearFormed: number;
  yearBorn: number;
  country: string;
  genres: Array<string> | undefined;
  styles: Array<string> | undefined;
  events: Array<EventData>;
  //albums: Array<AlbumData>;
}

export interface ExtendedArtistData extends ArtistData {
  albums: Array<AlbumData>;
}

export interface ArtistDataSimple {
  id: number;
  name: string;
}

export let defaultArtistData: ArtistData = {
  id: 0,
  name: '',
  photoUrl: '',
  websiteUrl: '',
  facebookUrl: '',
  bio: '',
  yearFormed: 0,
  yearBorn: 0,
  country: '',
  genres: [],
  styles: [],
  events: []
  //albums: []
};

export let defaultArtistDataSimple: ArtistDataSimple = {
  id: 0,
  name: ''
};