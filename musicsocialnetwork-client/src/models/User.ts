import { RatingData } from './Catalog';
import { EventData } from './Event';

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
  Events: Array<EventData>;
}

export interface ILoginState {
  data: LoginData;
}