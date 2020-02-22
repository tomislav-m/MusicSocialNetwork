const artistApiUrl: string = 'http://localhost:57415/api/Artists';
const albumApiUrl: string = 'http://localhost:57415/api/Albums';
const recsApiUrl: string = 'http://localhost:57415/api/Recommendations';

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

export async function getPopularAlbums() {
  try {
    const response = await fetch(`${albumApiUrl}/popular`, {
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

export async function getSimpleAlbums(ids: Array<number>) {
  try {
    const response = await fetch(`${albumApiUrl}/simple`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(ids)
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

export async function getArtistNames(ids: Array<number>) {
  try {
    const response = await fetch(`${artistApiUrl}/names`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(ids)
    });
    return response.json();
  } catch (err) {
    console.log(err);
  }
}

export async function addToCollection(userId: number, albumId: number) {
  try {
    const response = await fetch(`${albumApiUrl}/add-to-collection`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ userId, albumId })
    });
    return response.json();
  } catch (err) {
    console.log(err);
  }
}

export async function getRecommendations(userId: number) {
  try {
    const response = await fetch(`${recsApiUrl}/${userId}`, {
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

export async function getCollection(userId: number) {
  try {
    const response = await fetch(`${albumApiUrl}/collection/${userId}`, {
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