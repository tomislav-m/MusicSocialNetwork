import React from 'react';
import { Menu, Popup, Button, Form, Input, Icon } from 'semantic-ui-react';
import { inject, observer } from 'mobx-react';
import UserStore from '../../stores/UserStore';
import autobind from 'autobind-decorator';
import './Membership.css';

interface RegistrationProps {
  userStore?: UserStore;
}

interface RegistrationState {
  username: string;
  email: string;
  password: string;
  repeatPassword: string;
  isReadyToRegister: boolean;
}

@inject('userStore')
@observer
export class Registration extends React.Component<RegistrationProps, RegistrationState> {
  constructor(props: RegistrationProps) {
    super(props);

    this.state = {
      username: '',
      email: '',
      password: '',
      repeatPassword: '',
      isReadyToRegister: false
    };
  }

  componentDidUpdate(prevProps: RegistrationProps, prevState: RegistrationState) {
    if (this.state.email !== prevState.email || this.state.username !== prevState.username ||
      this.state.password !== prevState.password || this.state.repeatPassword !== prevState.repeatPassword) {
      this.setState({
        isReadyToRegister: this.readyToRegister()
      });
    }
  }

  public render() {
    const userStore = this.props.userStore;

    return (
      <div style={{ marginTop: '4px' }}>
        {
          !userStore?.isLoggedIn &&
          <Menu.Item header>
            <Popup
              position="bottom center"
              on="click"
              pinned
              trigger={<Button inverted compact>Sign up</Button>}
            >
              {this.registrationForm()}
            </Popup>
          </Menu.Item>
        }
      </div>
    );
  }

  @autobind
  handleUsernameChange(event: { target: HTMLInputElement }) {
    this.setState({
      username: event.target.value
    });
  }

  @autobind
  handleEmailChange(event: { target: HTMLInputElement }) {
    this.setState({
      email: event.target.value
    });
  }

  @autobind
  handlePasswordChange(event: { target: HTMLInputElement }) {
    this.setState({
      password: event.target.value
    });
  }

  @autobind
  handleRepeatPasswordChange(event: { target: HTMLInputElement }) {
    this.setState({
      repeatPassword: event.target.value
    });
  }

  @autobind
  readyToRegister() {
    return this.state.username !== '' && this.state.email.length > 5 &&
      this.state.password.length > 4 && this.state.repeatPassword === this.state.password;
  }

  private registrationForm = (): JSX.Element => {
    const userStore = this.props.userStore;
    const state = this.state;

    return (
      <span>
        <Form>
          <Form.Field>
            <label>Username</label>
            <input type="text" value={state.username} onChange={this.handleUsernameChange} disabled={userStore?.isLoading} />
          </Form.Field>
          <Form.Field>
            <label>E-mail</label>
            <input type="email" value={state.email} onChange={this.handleEmailChange} disabled={userStore?.isLoading} />
          </Form.Field>
          <Form.Field>
            <label>Password</label>
            <input type="password" value={state.password} onChange={this.handlePasswordChange} disabled={userStore?.isLoading} />
          </Form.Field>
          <Form.Field>
            <label>Repeat password</label>
            <Input type="password" value={state.repeatPassword} onChange={this.handleRepeatPasswordChange} disabled={userStore?.isLoading} />
          </Form.Field>
          <Button
            type="submit"
            disabled={this.state.isReadyToRegister === false}
            loading={userStore?.isLoading}
            onClick={this.handleRegister}>Sign up
        </Button>
          {
            userStore?.registerIsSuccess === false &&
            <div className="membership-error"><Icon name="times circle" />Registration unsuccessful!</div>
          }
        </Form>
      </span>
    );
  }

  @autobind
  private handleRegister() {
    const { username, email, password } = this.state;
    this.props.userStore?.register(username, email, password);
  }
}