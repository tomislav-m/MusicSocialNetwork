import { Track } from './Track';

export interface AlbumData {
  Id: number;
  Name: string;
  CoverArtUrl: string;
  Description: string;
  ArtistId: number;
  Style: string;
  Genre: string;
  Format: string;
  YearReleased: number;
  RatingData: AlbumRatingData;
  Tracks: Array<Track>;
}

export interface AlbumRatingData {
  AverageRating: number;
  RatingCount: number;
}

export let defaultAlbumRatingData: AlbumRatingData = {
  AverageRating: 0,
  RatingCount: 0
};

export let defaultAlbumData: AlbumData = {
  Id: 0,
  Name: '',
  CoverArtUrl: '',
  Description: '',
  ArtistId: 0,
  Style: '',
  Genre: '',
  Format: '',
  YearReleased: 0,
  RatingData: defaultAlbumRatingData,
  Tracks: []
};