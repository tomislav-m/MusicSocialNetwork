import { observable, action, computed } from 'mobx';
import { LoginData } from '../models/User';
import autobind from 'autobind-decorator';

export default class UserStore {
  @observable loginData: LoginData = {
    Username: '',
    Password: ''
  };

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

  @computed
  get isReadyToLogin(): boolean {
    return this.loginData.Password.length > 0 && this.loginData.Username.length > 0;
  }
}