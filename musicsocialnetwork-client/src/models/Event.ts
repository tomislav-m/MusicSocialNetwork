import { ArtistDataSimple } from './Artist';

export interface EventData {
  Id: number;
  Date: Date;
  VenueName: string;
  Headliners: Array<ArtistDataSimple>;
  Supporters: Array<ArtistDataSimple>;
}

export const defaultEvent: EventData = {
  Id: 0,
  Date: new Date(),
  VenueName: '',
  Headliners: [],
  Supporters: []
};