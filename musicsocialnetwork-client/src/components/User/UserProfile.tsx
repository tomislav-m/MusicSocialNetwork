import React from 'react';
import { inject, observer } from 'mobx-react';
import UserStore from '../../stores/UserStore';

interface UserProps {
  userStore: UserStore;
}

@inject('userStore')
@observer
export default class UserProfile extends React.Component<UserProps> {
  public render() {
    return (
      <div></div>
    );
  }
}