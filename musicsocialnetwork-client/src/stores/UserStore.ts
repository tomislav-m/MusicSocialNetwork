import { observable, action, computed } from 'mobx';
import { LoginData, UserData, defaultUserData, Comment } from '../models/User';
import autobind from 'autobind-decorator';
import { authenticateAsync, registerAsync } from '../actions/User/UserActions';
import { rateAlbum, getRatedAlbums, addToCollection, getArtistNames, getCollection } from '../actions/Music/MusicActions';
import { UserEvent, EventData } from '../models/Event';
import { getMarkedEvents, getEvents } from '../actions/Events/EventActions';

export default class UserStore {
  @observable loginData: LoginData = {
    id: 0,
    username: '',
    password: ''
  };

  @observable albumRatings: Array<any> = [];
  @observable collection: Array<any> = [];
  @observable userEvents: Array<UserEvent> = [];
  @observable events: Array<EventData> = [];

  @observable isLoading: boolean = false;
  @observable isAddingToCollection: boolean = false;

  @observable userData: UserData | undefined = undefined;

  @observable registerIsSuccess: boolean | undefined = undefined;
  @observable loginError: boolean | undefined = undefined;

  @observable simpleArtistsDict: { [id: number]: string } = {};

  @observable comments: Array<Comment> = [];

  @observable rateError: boolean = false;
  @observable collectionError: boolean = false;

  @observable buyError: boolean | undefined = undefined;

  @autobind
  @action
  handleUsernameChange(event: { target: HTMLInputElement }) {
    this.loginData.username = event.target.value;
    this.loginError = false;
  }

  @autobind
  @action
  handlePasswordChange(event: { target: HTMLInputElement }) {
    this.loginData.password = event.target.value;
    this.loginError = false;
  }

  @autobind
  @action
  handleLogin() {
    this.isLoading = true;
    authenticateAsync(this.loginData)
      .then(data => {
        this.isLoading = false;
        if (!data.token) {
          this.loginError = true;
          return;
        }
        this.loginError = false;
        this.userData = { ...defaultUserData, ...data };

        if (this.userData) {
          getRatedAlbums(this.userData.id)
            .then(albums => {
              if (this.userData) {
                this.userData.ratings = albums;
              }
            })
            .catch(err => console.log(err));
          getMarkedEvents(this.userData.id)
            .then((userEvents: Array<UserEvent>) => {
              if (userEvents) {
                this.userEvents = userEvents;
                getEvents(userEvents.map(x => x.eventId))
                  .then(async (events: Array<EventData>) => {
                    this.events = events;
                    const ids = events.map(x => [...x.headliners, ...x.supporters]).reduce((a, b) => a.concat(b), []);
                    this.simpleArtistsDict = { ...await getArtistNames(Array.from(new Set(ids))) };
                  });
              }
            });
          getCollection(this.userData.id)
            .then(result => {
              if (!result.exception) {
                this.collection = result.albumIds;
              }
            });
        }
      })
      .catch(err => console.log(err));
  }

  @autobind
  @action
  register(username: string, email: string, password: string) {
    this.isLoading = true;
    registerAsync({ username, email, password })
      .then((result) => {
        if (result.exception) {
          this.registerIsSuccess = false;
        } else {
          this.registerIsSuccess = true;
        }
      })
      .catch((err) => this.registerIsSuccess = false)
      .finally(() => this.isLoading = false);
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
    if (this.userData && ((prevRating && prevRating?.rating !== rating) || !prevRating)) {
      rateAlbum(albumId, rating, this.userData.id)
        .then(data => {
          if (data.exception) {
            this.rateError = true;
            console.log(data.exception);
          } else {
            const index = this.userData?.ratings.findIndex(x => x.albumId === albumId);
            if (index && index > -1 && this.userData?.ratings) {
              this.userData.ratings.splice(index, 1);
            }
            this.userData?.ratings.push({ albumId, rating, createdAt: new Date() });
            this.rateError = false;
          }
        })
        .catch(err => {
          this.rateError = true;
          console.log(err);
        });
    }
  }

  @autobind
  @action
  handleAddToCollection(albumId: number) {
    if (this.userData?.id === undefined) {
      return;
    }
    addToCollection(this.userData.id, albumId)
      .then(data => {
        if (data.exception) {
          console.log(data.exception);
          this.collectionError = true;
        } else {
          const index = this.collection.findIndex(x => x === albumId);
          if (index === -1) {
            this.collection.push(data.albumId);
          } else {
            this.collection.splice(index, 1);
          }

          this.collectionError = false;
        }
      })
      .catch(() => {
        this.collectionError = false;
      });
  }

  @autobind
  @action setBuyError(value: boolean) {
    this.buyError = value;
  }

  @computed
  get isReadyToLogin(): boolean {
    return this.loginData.password.length > 0 && this.loginData.username.length > 0;
  }

  @computed
  get isLoggedIn(): boolean {
    return this.userData !== undefined;
  }

  @computed
  get sortedRatings() {
    return this.albumRatings.slice().sort((x, y) => (new Date(y.createdAt)).getTime() - (new Date(x.createdAt)).getTime());
  }
}