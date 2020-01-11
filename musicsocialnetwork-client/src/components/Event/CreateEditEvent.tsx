import React from 'react';
import { ArtistDataSimple } from '../../models/Artist';
import { EventData } from '../../models/Event';

interface CreateEditEventProps {
  headliner?: ArtistDataSimple;
  supporter?: ArtistDataSimple;
  oldEvent?: EventData;

  onEventSave(): void;
}

export default class CreateEditEvent extends React.Component<CreateEditEventProps> {
  public render() {
    return (
      <div></div>
    );
  }
}