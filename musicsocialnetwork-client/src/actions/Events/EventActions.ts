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