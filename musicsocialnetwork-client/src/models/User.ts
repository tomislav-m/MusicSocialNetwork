import { RatingData } from './Catalog';

export interface LoginData {
  Id: number;
  Username: string;
  Password: string;
}

export interface UserData {
  Id: number;
  Username: string;
  Role: string;
  Ratings: Array<RatingData>;
}

export interface ILoginState {
  data: LoginData;
}