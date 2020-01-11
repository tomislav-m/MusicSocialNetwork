import { EventData } from './Event';

export interface ArtistSearchData {
  Id: number;
  Name: string;
  PhotoUrl: string;
}

export interface ArtistData {
  Id: number;
  Name: string;
  PhotoUrl: string;
  WebsiteUrl: string;
  FacebookUrl: string;
  Bio: string;
  YearFormed: number;
  YearBorn: number;
  Country: string;
  Genres: Array<string> | undefined;
  Styles: Array<string> | undefined;
  Events: Array<EventData>;
}

export interface ArtistDataSimple {
  Id: number;
  Name: string;
}

export let defaultArtistData: ArtistData = {
  Id: 0,
  Name: '',
  PhotoUrl: '',
  WebsiteUrl: '',
  FacebookUrl: '',
  Bio: '',
  YearFormed: 0,
  YearBorn: 0,
  Country: '',
  Genres: [],
  Styles: [],
  Events: []
};

export let defaultArtistDataSimple: ArtistDataSimple = {
  Id: 0,
  Name: ''
};