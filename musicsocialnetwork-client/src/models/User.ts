import { RatingData } from './Catalog';
import { EventData } from './Event';

export interface LoginData {
  id: number;
  username: string;
  password: string;
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
  username: string;
  email: string;
  password: string;
  role?: string;
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

export interface Comment {
  id: number;
  author: string;
  date: Date;
  text: string;
  pageType: string;
  parentId: number;
}