import { LoginData, UserData } from '../models/User';
import { eventData } from './EventDataMock';

export let loginData: Array<LoginData> = [
  {
    id: 1,
    username: 'User',
    password: 'admin'
  },
  {
    id: 2,
    username: 'User',
    password: 'user'
  }
];

export let userData: Array<UserData> = [
  {
    id: 1,
    username: 'User',
    role: 'administrator',
    ratings: [
      { albumId: 2, rating: 9, createdAt: new Date('2020-01-01') },
      { albumId: 1, rating: 10, createdAt: new Date() }
    ],
    events: eventData
  },
  {
    id: 2,
    username: 'User1',
    role: 'user',
    ratings: [],
    events: []
  }
];