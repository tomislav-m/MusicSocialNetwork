import { LoginData, UserData } from '../models/User';

export let loginData: Array<LoginData> = [
  {
    Id: 1,
    Username: 'Admin',
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
    Username: 'Admin',
    Role: 'administrator'
  },
  {
    Id: 2,
    Username: 'User',
    Role: 'user'
  }
];