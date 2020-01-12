import { EventData } from '../models/Event';

export let eventData: Array<EventData> = [
  {
    Id: 1,
    Date: new Date(),
    Headliners: [{ Id: 1, Name: 'Iron Maiden' }],
    Supporters: [{ Id: 2, Name: 'Dream Theater' }],
    VenueName: 'Arena Zagreb'
  }
];