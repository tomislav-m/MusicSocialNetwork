import { observable, action, computed } from 'mobx';
import { LoginData, UserData } from '../models/User';
import autobind from 'autobind-decorator';
import { loginData, userData } from '../data/UserDataMock';

export default class UserStore {
  @observable loginData: LoginData = {
    Id: 0,
    Username: '',
    Password: ''
  };

  @observable userData: UserData | undefined = undefined;

  @autobind
  @action
  handleUsernameChange(event: { target: HTMLInputElement }) {
    this.loginData.Username = event.target.value;
  }

  @autobind
  @action
  handlePasswordChange(event: { target: HTMLInputElement }) {
    this.loginData.Password = event.target.value;
  }

  @autobind
  @action
  handleLogin() {
    const user = loginData.find(x => x.Username === this.loginData.Username && x.Password === this.loginData.Password);
    if (user) {
      this.userData = userData.find(x => x.Id === user.Id);
      console.log(this.userData?.Username);
    }
  }

  @computed
  get isReadyToLogin(): boolean {
    return this.loginData.Password.length > 0 && this.loginData.Username.length > 0;
  }
}