import React from 'react';
import { AlbumData, defaultAlbumData } from '../../models/Album';
import { Form, Input, Button, Search, SearchProps } from 'semantic-ui-react';
import { artistData } from '../../data/SearchDataMock';
import { ArtistSearchData, ArtistDataSimple, defaultArtistDataSimple } from '../../models/Artist';
import _ from 'lodash';
import autobind from 'autobind-decorator';

interface CreateEditAlbumProps {
  albumData?: AlbumData;
  isEdit: boolean;

  onAlbumSave(): void;
}

interface CreateEditAlbumState {
  albumData: AlbumData;
  artistSearchData: Array<ArtistSearchData>;
  selectedArtist: ArtistDataSimple;
}

export default class CreateEditAlbum extends React.Component<CreateEditAlbumProps, CreateEditAlbumState> {
  constructor(props: CreateEditAlbumProps) {
    super(props);

    if (!props.albumData) {
      this.state = { albumData: defaultAlbumData, artistSearchData: [], selectedArtist: defaultArtistDataSimple };
    } else {
      this.state = { albumData: props.albumData, artistSearchData: [], selectedArtist: defaultArtistDataSimple };
    }
  }

  public render() {
    const album = this.props.albumData;

    return (
      <Form>
        <Form.Field>
          <label>Name</label>
          <Input type="date" value={album?.name} />
        </Form.Field>
        <Form.Field>
          <label>Artist</label>
          <Search
            size="mini"
            onSearchChange={_.debounce(this.handleSearch, 500)}
            onResultSelect={this.handleArtistSelect}
            results={this.mapSearchDataToResult(this.state.artistSearchData)}
            minCharacters={3}
          />
          <Input type="number" />
        </Form.Field>
        <Form.Field>
          <label>Photo URL</label>
          <Input type="text" value={album?.coverArtUrl} />
        </Form.Field>
        <Form.Field>
          <label>Website URL</label>
          <Input type="number" value={album?.yearReleased} />
        </Form.Field>
        <Form.Field>
          <label>Biography</label>
          <Input type="textarea" value={album?.description} />
        </Form.Field>
        <Button onClick={this.props.onAlbumSave}>Save</Button>
      </Form>
    );
  }

  @autobind
  private handleSearch(event: any, { value }: SearchProps) {
    if (value !== undefined && value.length > 2) {
      const filteredData = artistData.filter(x => x.name.toLowerCase().includes(value.toLowerCase()));
      this.setState({
        artistSearchData: filteredData
      });
    }
  }

  @autobind
  private handleArtistSelect(event: any, { result }: any) {
    const selectedArtist = this.state.artistSearchData.find(x => x.id === result.Id);
    this.setState({  });
  }

  @autobind
  handleChange(event: any, {value}: any) {
    const name = event.target.name;
    const albumData = {...this.state.albumData, [name]: value};
    this.setState({
      albumData
    });
  }

  private mapSearchDataToResult(searchData: Array<ArtistSearchData> | undefined) {
    if (searchData !== undefined) {
      const result: Array<any> = [];
      searchData.forEach(x => result.push({ title: x.name, image: x.photoUrl, id: x.id }));

      return result;
    }
  }
}