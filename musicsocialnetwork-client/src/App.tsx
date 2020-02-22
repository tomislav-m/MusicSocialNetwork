import React from 'react';
import 'semantic-ui-css/semantic.min.css';
import './App.css';
import { Menu, Container } from 'semantic-ui-react';
import Login from './components/Membership/Login';
import { Provider, observer } from 'mobx-react';
import UserStore from './stores/UserStore';
import SearchComponent from './components/Search/Search';
import SearchStore from './stores/SearchStore';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom';
import Artist from './components/Artist/Artist';
import ArtistStore from './stores/ArtistStore';
import Album from './components/Album/Album';
import UserProfile from './components/User/UserProfile';
import { Registration } from './components/Membership/Registration';
import Notification from './common/Notification';
import PopularAlbums from './components/PopularAlbums/PopularAlbums';

@observer
class App extends React.Component {
  private userStore: UserStore = new UserStore();
  private searchStore: SearchStore = new SearchStore();
  private artistStore: ArtistStore = new ArtistStore();

  render() {
    return (
      <Router>
        <Menu fixed="top" inverted>
          <Container>
            <Menu.Item header><Link to="/">Home</Link></Menu.Item>
            <Menu.Item>
              <Provider searchStore={this.searchStore}>
                <SearchComponent />
              </Provider>
            </Menu.Item>
            <Provider userStore={this.userStore}>
              <Login />
              <Registration />
            </Provider>
            {
              this.userStore.isLoggedIn &&
              <Menu.Item header onClick={this.userStore.handleLogout}>Logout</Menu.Item>
            }
          </Container>
        </Menu>

        <Container className="content-container">
          <Route path="/" exact component={PopularAlbums} />
          <Provider userStore={this.userStore}>
            <Route path="/Login" component={Login} />
          </Provider>
          <Provider artistStore={this.artistStore} userStore={this.userStore}>
            <Route path="/Artist/:id" component={Artist} />
            <Route path="/Album/:id" component={Album} />
            <Route path="/User/:id" component={UserProfile} />

            <Notification title="Registration" text="Registration successful!" positive={true} dimmed={true} active={this.userStore.registerIsSuccess === true} />
            <Notification
              active={this.userStore.buyError === true}
              dimmed={true}
              negative={true}
              text="Error buying ticket!"
              title="Error"
            />
            <Notification
              active={this.userStore.buyError === false}
              dimmed={true}
              positive={true}
              text="Ticket bought!"
              title="Success"
            />
          </Provider>
        </Container>
      </Router>
    );
  }
}

export default App;
