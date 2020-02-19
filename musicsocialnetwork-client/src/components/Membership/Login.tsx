import React from 'react';
import { Form, Button, Popup, Menu, Input, Icon } from 'semantic-ui-react';
import { inject, observer } from 'mobx-react';
import UserStore from '../../stores/UserStore';
import { Link } from 'react-router-dom';
import './Login.css';

interface LoginProps {
  userStore?: UserStore;
}

@inject('userStore')
@observer
export default class Login extends React.Component<LoginProps> {
  public render() {
    const userStore = this.props.userStore;

    return (
      <Menu.Item header>
        {userStore?.userData && userStore.isLoggedIn ? <Link to={`/User/${userStore.userData.id}`}>{userStore.userData.username}</Link>
          :
          <Popup
            position="bottom center"
            on="click"
            pinned
            trigger={<Button inverted compact>Login</Button>}
          >
            {this.loginForm()}
          </Popup>
        }
      </Menu.Item>
    );
  }

  private loginForm = (): JSX.Element => {
    const userStore = this.props.userStore;
    const loginError = userStore?.loginError;

    return (
      <Form>
        <Form.Field>
          <label>Username</label>
          <Input type="text" value={userStore?.loginData.username} onChange={userStore?.handleUsernameChange} disabled={userStore?.isLoading} error={loginError === true} />
        </Form.Field>
        <Form.Field>
          <label>Password</label>
          <Input type="password" value={userStore?.loginData.password} onChange={userStore?.handlePasswordChange} disabled={userStore?.isLoading} error={loginError === true} />
        </Form.Field>
        <Button
          type="submit"
          disabled={!userStore?.isReadyToLogin}
          loading={userStore?.isLoading}
          onClick={userStore?.handleLogin}
          negative={loginError === true}>Log in
        </Button>
        {
          loginError === true &&
          <div className="login-error"><Icon name="warning sign" />Wrong username or password!</div>
        }
      </Form>
    );
  }
}