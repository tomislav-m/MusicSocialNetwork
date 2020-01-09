export interface LoginData {
  Id: number;
  Username: string;
  Password: string;
}

export interface UserData {
  Id: number;
  Username: string;
  Role: string;
}

export interface ILoginState {
  data: LoginData;
}