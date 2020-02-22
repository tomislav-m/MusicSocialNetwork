import React from 'react';
import { ArtistData, defaultArtistData } from '../../models/Artist';
import { Form, Input, Button, TextArea } from 'semantic-ui-react';
import autobind from 'autobind-decorator';

interface CreateEditArtistProps {
  artistData?: ArtistData;
  isEdit: boolean;

  onArtistSave(): void;
}

interface CreateEditArtistState {
  artistData: ArtistData;
}

export default class CreateEditArtist extends React.Component<CreateEditArtistProps, CreateEditArtistState> {
  constructor(props: CreateEditArtistProps) {
    super(props);

    if (!props.artistData) {
      this.state = { artistData: defaultArtistData };
    } else {
      this.state = { artistData: props.artistData };
    }
  }

  public render() {
    const artist = this.state.artistData;

    return (
      <Form>
        <Form.Field>
          <label>Name</label>
          <Input name="Name" type="text" value={artist?.name} onChange={this.handleChange} />
        </Form.Field>
        <Form.Field>
          <label>Photo URL</label>
          <Input name="PhotoUrl" type="text" value={artist?.photoUrl} onChange={this.handleChange} />
        </Form.Field>
        <Form.Field>
          <label>Website URL</label>
          <Input name="WebsiteUrl" type="text" value={artist?.websiteUrl} onChange={this.handleChange} />
        </Form.Field>
        <Form.Field>
          <label>Facebook URL</label>
          <Input name="FacebookUrl" type="text" value={artist?.facebookUrl} onChange={this.handleChange} />
        </Form.Field>
        <Form.Field>
          <label>Country</label>
          <Input name="Country" type="text" value={artist?.country} onChange={this.handleChange} />
        </Form.Field>
        <Form.Field>
          <label>Year Formed</label>
          <Input name="YearFormed" type="number" value={artist?.yearFormed} onChange={this.handleChange} />
        </Form.Field>
        <Form.Field>
          <label>Year Born</label>
          <Input name="YearBorn" type="number" value={artist?.yearBorn} onChange={this.handleChange} />
        </Form.Field>
        <Form.Field>
          <label>Biography</label>
          <TextArea name="Bio" value={artist?.bio} onChange={this.handleChange} />
        </Form.Field>
        <Button onClick={this.props.onArtistSave}>Save</Button>
      </Form>
    );
  }

  @autobind
  handleChange(event: any, {value}: any) {
    const name = event.target.name;
    const artistData = {...this.state.artistData, [name]: value};
    this.setState({
      artistData
    });
  }
}