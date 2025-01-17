import { Track } from './Track';
import { ArtistDataSimple } from './Artist';

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

export interface AlbumDataSimple {
  id: number;
  name: string;
  coverArtUrl: string;
  artist: ArtistDataSimple;
  rating: number;
}

export interface AlbumRatingData {
  averageRating: number;
  ratingCount: number;
}

export let defaultAlbumRatingData: AlbumRatingData = {
  averageRating: 0,
  ratingCount: 0
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