import React from 'react';
import { Form, Button } from 'semantic-ui-react';
import { inject, observer } from 'mobx-react';
import UserStore from '../../stores/UserStore';

interface LoginProps {
  userStore?: UserStore;
}

@inject('userStore')
@observer
export default class Login extends React.Component<LoginProps> {
  public render() {
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
        <Button type="submit" disabled={!userStore?.isReadyToLogin}>Submit</Button>
      </Form>
    );
  }
}