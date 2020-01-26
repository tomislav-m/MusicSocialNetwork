import { ArtistSearchData } from '../models/Artist';
import { observable, action } from 'mobx';
import autobind from 'autobind-decorator';
import { SearchProps } from 'semantic-ui-react';
import { searchArtist } from '../actions/Music/MusicActions';

export default class SearchStore {

  searchResult: Array<ArtistSearchData> = observable([]);
  @observable searchTerm: string = '';
  @observable isSearchLoading: boolean = false;

  @autobind
  @action
  handleSearchChange(e: React.MouseEvent, { value }: SearchProps) {
    if (value !== undefined && value.length > 2) {
      this.searchTerm = value;
      this.isSearchLoading = true;

      searchArtist(this.searchTerm)
        .then((result: Array<any>) => {
          this.searchResult = result;
        })
        .finally(() => this.isSearchLoading = false);
    }
  }
}