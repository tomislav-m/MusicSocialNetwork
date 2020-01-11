import React from 'react';
import { ArtistData, defaultArtistData } from '../../models/Artist';
import { artistData } from '../../data/SearchDataMock';

interface CreateEditArtistProps {
  artistData?: ArtistData;

  onArtistSave(): void;
}

export default class CreateEditArtist extends React.Component<CreateEditArtistProps> {
  constructor(props: CreateEditArtistProps) {
    super(props);

    if (!props.artistData) {
      props.artistData = {...defaultArtistData};
    }
  }

  public render() {
    return (
      <div></div>
    );
  }
}