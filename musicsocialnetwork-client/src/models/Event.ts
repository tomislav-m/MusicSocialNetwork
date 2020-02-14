export interface EventData {
  id: number;
  date: Date;
  venue: string;
  headliners: Array<number>;
  supporters: Array<number>;
}

export const defaultEvent: EventData = {
  id: 0,
  date: new Date(),
  venue: '',
  headliners: [],
  supporters: []
};

export interface UserEvent {
  eventId: number;
  markEventType: number;
  venue: string;
  date: Date;
}