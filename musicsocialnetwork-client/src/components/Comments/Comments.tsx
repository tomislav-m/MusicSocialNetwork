import React from 'react';
import { Comment, Form, Button } from 'semantic-ui-react';
import { inject, observer } from 'mobx-react';
import UserStore from '../../stores/UserStore';
import './Comments.css';
import autobind from 'autobind-decorator';
import { addComment, getComments } from '../../actions/User/UserActions';
import { Comment as CommentModel } from '../../models/User';

interface ICommentsProps {
  userStore?: UserStore;
  pageType: string;
  parentId: number;
}

interface ICommentState {
  text: string;
  isReplying: boolean;
  comments: Array<CommentModel>;
}

@inject('userStore')
@observer
export default class Comments extends React.Component<ICommentsProps, ICommentState> {
  constructor(props: ICommentsProps) {
    super(props);

    this.state = {
      text: '',
      isReplying: false,
      comments: []
    };
  }

  componentDidMount() {
    this.handleGetComments();
  }

  render() {
    const { isReplying, comments } = this.state;
    const userData = this.props.userStore?.userData;

    return (
      <Comment.Group>
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
          {
            userData?.id &&
            <div>
              <Form.TextArea width="14" rows={5} onInput={this.handleCommentInput} disabled={isReplying} />
              <Button content="Add reply" labelPosition="left" icon="edit" onClick={this.handleAddComment} disabled={isReplying} loading={isReplying} />
            </div>
          }
        </Form>
      </Comment.Group>
    );
  }

  @autobind
  private handleCommentInput(event: any, { value }: any) {
    this.setState({ text: value });
  }

  @autobind
  private handleGetComments() {
    getComments(this.props.pageType, this.props.parentId)
      .then(result => {
        this.setState({
          comments: result
        });
      });
  }

  @autobind
  private handleAddComment() {
    const userData = this.props.userStore?.userData;
    const { pageType, parentId } = this.props;
    const { text } = this.state;

    this.setState({
      isReplying: true
    });
    addComment({ author: userData?.id || 0, date: new Date(), pageType, text, id: 0, parentId })
      .then((result) => {
        if (!result.exception) {
          this.props.userStore?.comments.push(result);
          this.setState({
            text: ''
          });
        }
      })
      .finally(() =>
        this.setState({
          isReplying: false
        })
      );
  }
}