import React from 'react';
import { inject, observer } from 'mobx-react';
import UserStore from '../../stores/UserStore';
import { Grid, Divider, Modal, Table, Button } from 'semantic-ui-react';
import ArtistStore from '../../stores/ArtistStore';
import { albumData } from '../../data/AlbumDataMock';
import { artistData } from '../../data/ArtistDataMock';

interface UserProps {
  userStore?: UserStore;
  artistStore?: ArtistStore;
}

@inject('userStore')
@inject('artistStore')
@observer
export default class UserProfile extends React.Component<UserProps> {
  albumRatings: Array<any> | undefined = this.props.userStore?.userData?.Ratings.map(rating => {
    const album = albumData.find(x => x.Id === rating.AlbumId);
    const artist = artistData.find(x => x.Id === album?.ArtistId);
    return {
      album: album?.Name,
      artist: artist?.Name,
      rating: rating.Rating,
      yearReleased: album?.YearReleased,
      coverUrl: album?.CoverArtUrl,
      RatedAt: rating.RatedAt
    };
  });

  public render() {
    return (
      <Grid>
        <Grid.Row>
          <h2>{this.props.userStore?.userData?.Username}</h2>
        </Grid.Row>
        <Divider horizontal />
        <Grid.Row divided>
          <Grid.Column width="8">
            <Modal trigger={<Button>Album ratings</Button>}>
              <Modal.Header>Album ratings</Modal.Header>
              <Modal.Content>
                <Table striped compact>
                  <Table.Body>
                    {
                      this.albumRatings?.map(rating =>
                        <Table.Row>
                          <Table.Cell><img width="50px" src={rating.coverUrl} alt="cover" /></Table.Cell>
                        </Table.Row>
                      )
                    }
                  </Table.Body>
                </Table>
              </Modal.Content>
            </Modal>
          </Grid.Column>
          <Grid.Column width="8"></Grid.Column>
        </Grid.Row>
      </Grid>
    );
  }
}