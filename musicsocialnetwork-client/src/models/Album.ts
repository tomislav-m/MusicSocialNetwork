import { Track } from './Track';

export interface AlbumData {
  id: number;
  name: string;
  coverArtUrl: string;
  description: string;
  artistId: number;
  style: string;
  genre: string;
  format: string;
  yearReleased: number;
  ratingData: AlbumRatingData;
  tracks: Array<Track>;
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
  id: 0,
  name: '',
  coverArtUrl: '',
  description: '',
  artistId: 0,
  style: '',
  genre: '',
  format: '',
  yearReleased: 0,
  ratingData: defaultAlbumRatingData,
  tracks: []
};