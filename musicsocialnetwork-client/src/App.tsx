import React from 'react';
import 'semantic-ui-css/semantic.min.css';
import './App.css';
import { Menu, Container, Popup, Button } from 'semantic-ui-react';
import Login from './components/Login/Login';
import { Provider } from 'mobx-react';
import UserStore from './stores/UserStore';

class App extends React.Component {
  private userStore: UserStore = new UserStore();

  render() {
    return (
      <div>
        <Menu fixed="top" inverted>
          <Container>
            <Menu.Item onClick={() => console.log('Home')} header>Home</Menu.Item>
            <Menu.Item></Menu.Item>
            <Menu.Item>
              <Provider userStore={this.userStore}>
                <Popup
                  // basic
                  position="bottom center"
                  on="click"
                  pinned
                  content={<Login />}
                  trigger={
                    <Button inverted onClick={() => console.log('Login')}>Login</Button>
                  }
                />
              </Provider>
            </Menu.Item>
          </Container>
        </Menu>
      </div>
    );
  }
}

export default App;
