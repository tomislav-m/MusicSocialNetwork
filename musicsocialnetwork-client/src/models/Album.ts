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