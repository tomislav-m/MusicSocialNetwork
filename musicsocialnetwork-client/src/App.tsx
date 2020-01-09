import React from 'react';
import 'semantic-ui-css/semantic.min.css';
import './App.css';
import { Menu, Container } from 'semantic-ui-react';
import Login from './components/Login/Login';
import { Provider } from 'mobx-react';
import UserStore from './stores/UserStore';
import SearchComponent from './components/Search/Search';
import SearchStore from './stores/SearchStore';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom';
import Artist from './components/Artist/Artist';
import ArtistStore from './stores/ArtistStore';
import Album from './components/Album/Album';

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
            <Menu.Item>
              <Provider userStore={this.userStore}>
                <Login />
              </Provider>
            </Menu.Item>
          </Container>
        </Menu>

        <Container className="content-container">
          <Provider userStore={this.userStore}>
            <Route path="/Login" component={Login} />
          </Provider>
          <Provider artistStore={this.artistStore} userStore={this.userStore}>
            <Route path="/Artist/:id" component={Artist} />
            <Route path="/Album/:id" component={Album} />
          </Provider>
        </Container>
      </Router>
    );
  }
}

export default App;
