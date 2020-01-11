import React from 'react';
import { AlbumData, defaultAlbumData } from '../../models/Album';

interface CreateEditAlbumProps {
  albumData?: AlbumData;

  onAlbumSave(): void;
}

export default class CreateEditAlbum extends React.Component<CreateEditAlbumProps> {
  constructor(props: CreateEditAlbumProps) {
    super(props);

    if (!props.albumData) {
      props.albumData = {...defaultAlbumData};
    }
  }

  public render() {
    return (
      <div></div>
    );
  }
}