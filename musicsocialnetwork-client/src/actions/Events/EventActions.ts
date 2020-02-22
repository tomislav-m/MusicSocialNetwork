import { EventData } from '../../models/Event';

const eventApiUrl: string = 'http://localhost:57415/api/Events';
const ticketingApiUrl: string = 'http://localhost:57415/api/Ticketing';

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

export async function getEvents(eventIds: Array<number>) {
  try {
    const response = await fetch(`${eventApiUrl}/more`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(eventIds)
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

export async function editEvent(event: EventData) {
  try {
    const response = await fetch(`${eventApiUrl}/${event.id}`, {
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

export async function markEvent(userId: number, eventId: number, markEventType: number) {
  try {
    const response = await fetch(`${eventApiUrl}/mark`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ userId, eventId, markEventType })
    });
    return response.json();
  } catch (err) {
    console.log(err);
  }
}

export async function getMarkedEvents(userId: number) {
  try {
    const response = await fetch(`${eventApiUrl}/marked-events/${userId}`, {
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

export async function buyTickets(userId: number, eventId: number, count: number) {
  try {
    const response = await fetch(`${eventApiUrl}/buy`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ userId, eventId, count })
    });
    return response.json();
  } catch (err) {
    console.log(err);
  }
}

export async function getTickets(eventId: number) {
  try {
    const response = await fetch(`${ticketingApiUrl}/${eventId}`, {
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

export async function addTickets(eventId: number, ticketsOverall: number, price: number, currency: string) {
  try {
    const response = await fetch(`${ticketingApiUrl}/buy`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ eventId, ticketsOverall, price, currency })
    });
    return response.json();
  } catch (err) {
    console.log(err);
  }
}