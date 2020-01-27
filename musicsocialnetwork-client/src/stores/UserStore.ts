import { observable, action, computed } from 'mobx';
import { LoginData, UserData, defaultUserData } from '../models/User';
import autobind from 'autobind-decorator';
import { authenticateAsync } from '../actions/User/UserActions';
import { rateAlbum, getRatedAlbums } from '../actions/Music/MusicActions';

export default class UserStore {
  @observable loginData: LoginData = {
    Id: 0,
    Username: '',
    Password: ''
  };

  @observable isLoading: boolean = false;

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
    this.isLoading = true;
    authenticateAsync(this.loginData)
      .then(data => {
        this.userData = { ...defaultUserData, ...data };
        this.isLoading = false;

        if (this.userData) {
          getRatedAlbums(this.userData.id)
            .then(albums => {
              if (this.userData) {
                this.userData.ratings = albums;
              }
            })
            .catch(err => console.log(err));
        }
      })
      .catch(err => console.log(err));
  }

  @autobind
  @action
  handleLogout() {
    this.isLoading = false;
    this.userData = undefined;
  }

  @autobind
  @action
  rateAlbum(albumId: number, rating: number) {
    const prevRating = this.userData?.ratings.find(x => x.albumId === albumId);
    if (this.userData && (prevRating && prevRating?.rating !== rating || !prevRating)) {
      rateAlbum(albumId, rating, this.userData.id)
        .then(data => {
          if (data.exception) {
            console.log(data.exception);
          } else {
            this.userData?.ratings.push({ albumId, rating, createdAt: new Date() });
          }
        })
        .catch(err => console.log(err));
    }
  }

  @computed
  get isReadyToLogin(): boolean {
    return this.loginData.Password.length > 0 && this.loginData.Username.length > 0;
  }

  @computed
  get isLoggedIn(): boolean {
    return this.userData !== undefined;
  }
}