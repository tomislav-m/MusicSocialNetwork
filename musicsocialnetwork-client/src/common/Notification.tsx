import React from 'react';
import { Message } from 'semantic-ui-react';
import autobind from 'autobind-decorator';

interface INotificationProps {
  title?: string;
  text?: string;
}

interface INotificationState {
  visible: boolean;
}

export default class Notification extends React.Component<INotificationProps, INotificationState> {
  constructor(props: INotificationProps) {
    super(props);

    this.state = {
      visible: true
    };
  }

  render() {
    const { title, text } = this.props;
    const { visible } = this.state;
    return (
      <span>
        {
          visible &&
          <Message onDismiss={this.dismissMessage}>
            {
              title && <Message.Header>{title}</Message.Header>
            }
            <p>{text}</p>
          </Message>
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
}