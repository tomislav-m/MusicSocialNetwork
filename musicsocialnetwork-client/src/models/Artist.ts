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
}