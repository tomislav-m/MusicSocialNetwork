import React from 'react';
import { Form, Button } from 'semantic-ui-react';
import { ILoginState } from '../../models/User';
import autobind from 'autobind-decorator';

export default class Login extends React.Component<{}, ILoginState> {
  public state: ILoginState = {
    data: { Username: '', Password: '' }
  };

  @autobind
  private _handleUsernameChange(event: any) {
    const newUsername = event.target.value;
    const data = {...this.state.data, Username: newUsername};
    this.setState({ data });
  }

  @autobind
  private _handlePasswordChange(event: any) {
    const newPassword = event.target.value;
    const data = {...this.state.data, Password: newPassword};
    this.setState({ data });
  }

  public render() {
    return (
      <Form>
        <Form.Field>
          <label>Username</label>
          <input type="text" value={this.state.data.Username} onChange={this._handleUsernameChange} />
        </Form.Field>
        <Form.Field>
          <label>Password</label>
          <input type="password" value={this.state.data.Password} onChange={this._handlePasswordChange} />
        </Form.Field>
        <Button type="submit">Submit</Button>
      </Form>
    );
  }
}