const artistApiUrl: string = 'http://localhost:57415/api/Artists';
const albumApiUrl: string = 'http://localhost:57415/api/Albums';

export async function searchArtist(searchTerm: string) {
  try {
    const response = await fetch(`${artistApiUrl}/search/${searchTerm}`, {
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

export async function getArtist(id: number) {
  try {
    const response = await fetch(`${artistApiUrl}/${id}`, {
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

export async function getAlbum(id: number) {
  try {
    const response = await fetch(`${albumApiUrl}/${id}`, {
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

export async function rateAlbum(albumId: number, rating: number, userId: number) {
  try {
    const response = await fetch(`${albumApiUrl}/rate`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ albumId, rating, userId })
    });
    return response.json();
  } catch (err) {
    console.log(err);
  }
}

export async function getRatedAlbums(userId: number) {
  try {
    const response = await fetch(`${albumApiUrl}/rated/${userId}`, {
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

export async function getAverageRating(albumId: number) {
  try {
    const response = await fetch(`${albumApiUrl}/average-rating/${albumId}`, {
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