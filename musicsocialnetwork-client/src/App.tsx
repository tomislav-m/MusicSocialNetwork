import React from 'react';
import 'semantic-ui-css/semantic.min.css';
import './App.css';
import { Menu, Container, Popup, Button } from 'semantic-ui-react';
import Login from './components/Login/Login';

const App: React.FC = () => {
  return (
    <div>
      <Menu fixed="top" inverted>
        <Container>
          <Menu.Item onClick={() => console.log('Home')} header>Home</Menu.Item>
          <Menu.Item></Menu.Item>
          <Menu.Item>
            <Popup
              // basic
              position="bottom center"
              on="click"
              pinned
              content={<Login/>}
              trigger={
                <Button inverted onClick={() => console.log('Login')}>Login</Button>
              }
            />
          </Menu.Item>
        </Container>
      </Menu>
    </div>
  );
};

export default App;
