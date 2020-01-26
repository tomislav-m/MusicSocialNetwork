import React from 'react';
import { Search, SearchResultData } from 'semantic-ui-react';
import _ from 'lodash';
import { observer, inject } from 'mobx-react';
import { ArtistSearchData } from '../../models/Artist';
import SearchStore from '../../stores/SearchStore';
import { Redirect } from 'react-router-dom';
import autobind from 'autobind-decorator';

interface SearchProps {
  searchStore?: SearchStore;
}

interface SearchState {
  redirect: boolean;
  resultId: number;
}

@inject('searchStore')
@observer
export default class SearchComponent extends React.Component<SearchProps, SearchState> {
  constructor(props: SearchProps) {
    super(props);
    this.state = { redirect: false, resultId: 0 };
  }

  componentDidUpdate(prevProps: SearchProps, prevState: SearchState) {
    if (prevState.redirect !== this.state.redirect && this.state.redirect === true) {
      this.setState({ redirect: false });
    }
  }

  public render() {
    const searchStore = this.props.searchStore;
    const id = this.state.resultId;

    return (
      <div>
        <Search
          size="mini"
          loading={searchStore?.isSearchLoading}
          onSearchChange={searchStore && _.debounce(searchStore.handleSearchChange, 500)}
          results={this.mapSearchDataToResult(searchStore?.searchResult)}
          onResultSelect={this.openSelectedResult}
          minCharacters={3}
        />
        {
          this.state.redirect &&
          <Redirect to={`/Artist/${id}`} />
        }
      </div>
    );
  }

  private reset() {
    this.setState({ redirect: false });
  }

  private mapSearchDataToResult = (searchData: Array<ArtistSearchData> | undefined) => {
    if (searchData !== undefined) {
      const result: Array<any> = [];
      searchData.map(x => result.push({ title: x.name, image: x.photoUrl, id: x.id }));

      return result;
    }
  }

  @autobind
  private openSelectedResult(event: any, data: SearchResultData) {
    event.target.value = '';
    this.setState({ redirect: true, resultId: data.result.id });
  }
}