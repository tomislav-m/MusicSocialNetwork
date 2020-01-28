import { EventData } from '../../models/Event';

const eventApiUrl: string = 'http://localhost:57415/api/Events';

export async function getEventsByArtist(artistId: number) {
  try {
    const response = await fetch(`${eventApiUrl}/artist/${artistId}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json'
      }
    });
    return response.json();
  } catch (err) {
    console.log(err);
  }
}

export async function createEvent(event: EventData) {
  try {
    const response = await fetch(`${eventApiUrl}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(event)
    });
    return response.json();
  } catch (err) {
    console.log(err);
  }
}