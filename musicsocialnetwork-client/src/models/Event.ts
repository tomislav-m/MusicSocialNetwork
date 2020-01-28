export interface EventData {
  id: number;
  date: Date;
  venueName: string;
  headliners: Array<number>;
  supporters: Array<number>;
}

export const defaultEvent: EventData = {
  id: 0,
  date: new Date(),
  venueName: '',
  headliners: [],
  supporters: []
};