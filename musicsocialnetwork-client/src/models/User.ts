import { RatingData } from './Catalog';
import { EventData } from './Event';

export interface LoginData {
  Id: number;
  Username: string;
  Password: string;
}

export interface UserData {
  id: number;
  username: string;
  role: string;
  ratings: Array<RatingData>;
  events: Array<EventData>;
  token?: string;
}

export interface RegisterData {
  Username: string;
  Password: string;
  Role: string;
}

export interface ILoginState {
  data: LoginData;
}

export let defaultUserData: UserData = {
  id: 0,
  username: '',
  role: '',
  events: [],
  ratings: []
};