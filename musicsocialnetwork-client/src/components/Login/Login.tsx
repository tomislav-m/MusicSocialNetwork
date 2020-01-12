import React from 'react';
import { Form, Button, Popup } from 'semantic-ui-react';
import { inject, observer } from 'mobx-react';
import UserStore from '../../stores/UserStore';
import { Link } from 'react-router-dom';

interface LoginProps {
  userStore?: UserStore;
}

@inject('userStore')
@observer
export default class Login extends React.Component<LoginProps> {
  public render() {
    const userStore = this.props.userStore;

    return (
      <div>
        {userStore?.userData ? <Link to={`/User/${userStore.userData.Id}`}>{userStore.userData.Username}</Link> :
          <Popup
            position="bottom center"
            on="click"
            pinned
            trigger={<Button inverted compact>Login</Button>}
          >
            {this.loginForm()}
          </Popup>
        }
      </div>
    );
  }

  private loginForm = (): JSX.Element => {
    const userStore = this.props.userStore;

    return (
      <Form>
        <Form.Field>
          <label>Username</label>
          <input type="text" value={userStore?.loginData.Username} onChange={userStore?.handleUsernameChange} />
        </Form.Field>
        <Form.Field>
          <label>Password</label>
          <input type="password" value={userStore?.loginData.Password} onChange={userStore?.handlePasswordChange} />
        </Form.Field>
        <Button
          type="submit"
          disabled={!userStore?.isReadyToLogin}
          onClick={userStore?.handleLogin}>Submit
        </Button>
      </Form>
    );
  }
}