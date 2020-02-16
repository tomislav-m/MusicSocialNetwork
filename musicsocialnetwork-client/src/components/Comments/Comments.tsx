import React from 'react';
import { Comment, Header, Form, Button } from 'semantic-ui-react';
import { inject, observer } from 'mobx-react';
import UserStore from '../../stores/UserStore';
import './Comments.css';

interface ICommentsProps {
  userStore?: UserStore;
}

@inject('userStore')
@observer
export default class Comments extends React.Component<ICommentsProps> {

  render() {
    const { userStore } = this.props;
    const comments = userStore?.comments || [];

    return (
      <Comment.Group>
        <Header as="h3" dividing>
          Comments
          </Header>
        {
          comments.map(comment =>
            <Comment key={comment.id}>
              <Comment.Content>
                <Comment.Author as="a">{comment.author}</Comment.Author>
                <Comment.Metadata>{comment.date}</Comment.Metadata>
                <Comment.Text>{comment.text}</Comment.Text>
              </Comment.Content>
            </Comment>
          )
        }
        <Form reply>
          <Form.TextArea rows={5} />
          <Button content="Add reply" labelPosition="left" icon="edit" />
        </Form>
      </Comment.Group>
    );
  }
}