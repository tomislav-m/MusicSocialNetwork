import React from 'react';
import { Message, Dimmer } from 'semantic-ui-react';
import autobind from 'autobind-decorator';
import { observer, inject } from 'mobx-react';
import UserStore from '../stores/UserStore';

interface INotificationProps {
  title?: string;
  text?: string;
  dimmed: boolean;
  active: boolean;
  positive?: boolean;
  negative?: boolean;
  userStore?: UserStore;
}

interface INotificationState {
  visible: boolean;
}

@inject('userStore')
@observer
export default class Notification extends React.Component<INotificationProps, INotificationState> {
  constructor(props: INotificationProps) {
    super(props);

    this.state = {
      visible: false
    };
  }

  componentDidMount() {
    this.setState({
      visible: this.props.active
    });
  }

  componentDidUpdate(prevProps: INotificationProps) {
    if (prevProps.active !== this.props.active) {
      this.setState({
        visible: this.props.active
      });
    }
  }

  render() {
    return (
      <span>
        {
          this.props.dimmed ?
            <Dimmer active={this.state.visible} onClickOutside={this.dismissMessage}>
              {this.renderMessage()}
            </Dimmer>
            :
            this.renderMessage(this.props.dimmed)
        }
      </span>
    );
  }

  @autobind
  private dismissMessage() {
    this.setState({
      visible: false
    });
  }

  @autobind
  renderMessage(dimmed: boolean = false) {
    const { title, text, active, positive, negative } = this.props;
    let props = {};
    if (dimmed) {
      props = { positive, negative, onDismiss: this.dismissMessage, visible: active };
    } else {
      props = { positive, negative, visible: active };
    }
    return (
      <span>
        {
          active &&
          <Message {...props}>
            {
              title && <Message.Header>{title}</Message.Header>
            }
            <p>{text}</p>
          </Message>
        }
      </span>
    );
  }
}