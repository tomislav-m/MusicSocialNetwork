import { ArtistSearchData } from '../models/Artist';
import { observable, action } from 'mobx';
import autobind from 'autobind-decorator';
import { SearchProps } from 'semantic-ui-react';
import { artistData } from '../data/SearchDataMock';

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
      const filteredData = artistData.filter(x => x.Name.toLowerCase().includes(value.toLowerCase()));
      this.searchResult = filteredData;
      setTimeout(() => {
        this.isSearchLoading = false;
      }, 500);
    }
  }
}