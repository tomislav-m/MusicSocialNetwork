import React from 'react';
import { ArtistData, defaultArtistData } from '../../models/Artist';
import { Form, Input, Button, TextArea } from 'semantic-ui-react';
import autobind from 'autobind-decorator';
import { createArtist } from '../../actions/Music/MusicActions';
import { Redirect } from 'react-router-dom';

interface CreateEditArtistProps {
  artistData?: ArtistData;
  isEdit: boolean;
}

interface CreateEditArtistState {
  artistData: ArtistData;
  isLoading: boolean;
  redirectAddress: string;
}

export default class CreateEditArtist extends React.Component<CreateEditArtistProps, CreateEditArtistState> {
  constructor(props: CreateEditArtistProps) {
    super(props);

    if (!props.artistData) {
      this.state = {
        artistData: defaultArtistData,
        isLoading: false,
        redirectAddress: ''
      };
    } else {
      this.state = {
        artistData: props.artistData,
        isLoading: false,
        redirectAddress: ''
      };
    }
  }

  public render() {
    const { artistData, isLoading, redirectAddress } = this.state;

    return (
      <div>
        <Form>
          <Form.Field>
            <label>Name</label>
            <Input name="name" type="text" value={artistData?.name} onChange={this.handleChange} />
          </Form.Field>
          <Form.Field>
            <label>Photo URL</label>
            <Input name="photoUrl" type="text" value={artistData?.photoUrl} onChange={this.handleChange} />
          </Form.Field>
          <Form.Field>
            <label>Website URL</label>
            <Input name="websiteUrl" type="text" value={artistData?.websiteUrl} onChange={this.handleChange} />
          </Form.Field>
          <Form.Field>
            <label>Facebook URL</label>
            <Input name="facebookUrl" type="text" value={artistData?.facebookUrl} onChange={this.handleChange} />
          </Form.Field>
          <Form.Field>
            <label>Country</label>
            <Input name="country" type="text" value={artistData?.country} onChange={this.handleChange} />
          </Form.Field>
          <Form.Field>
            <label>Year Formed</label>
            <Input name="yearFormed" type="number" value={artistData?.yearFormed} onChange={this.handleChange} />
          </Form.Field>
          <Form.Field>
            <label>Year Born</label>
            <Input name="yearBorn" type="number" value={artistData?.yearBorn} onChange={this.handleChange} />
          </Form.Field>
          <Form.Field>
            <label>Biography</label>
            <TextArea name="bio" value={artistData?.bio} onChange={this.handleChange} />
          </Form.Field>
          <Button
            onClick={this.handleSave}
            disabled={isLoading}
            loading={isLoading}
          >Save</Button>
        </Form>
        {
          redirectAddress.length > 0 &&
          <Redirect to={redirectAddress} />
        }
      </div>
    );
  }

  @autobind
  handleChange(event: any, { value }: any) {
    const name = event.target.name;
    const artistData = { ...this.state.artistData, [name]: value };
    this.setState({
      artistData
    });
  }

  @autobind
  handleSave() {
    this.setState({
      isLoading: true
    });
    if (!this.props.isEdit) {
      createArtist(this.state.artistData)
        .then(result => {
          if (!result.exception) {
            const redirectAddress = `/Artist/${result.id}`;
            this.setState({
              redirectAddress
            });
          }
        })
        .finally(() => this.setState({ isLoading: false }));
    }
  }
}