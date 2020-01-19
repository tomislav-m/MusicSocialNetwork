import { LoginData, UserData } from '../models/User';
import { eventData } from './EventDataMock';

export let loginData: Array<LoginData> = [
  {
    Id: 1,
    Username: 'User',
    Password: 'admin'
  },
  {
    Id: 2,
    Username: 'User',
    Password: 'user'
  }
];

export let userData: Array<UserData> = [
  {
    Id: 1,
    Username: 'User',
    Role: 'administrator',
    Ratings: [
      { AlbumId: 2, Rating: 9, RatedAt: new Date('2020-01-01') },
      { AlbumId: 1, Rating: 10, RatedAt: new Date() }
    ],
    Events: eventData
  },
  {
    Id: 2,
    Username: 'User1',
    Role: 'user',
    Ratings: [],
    Events: []
  }
];