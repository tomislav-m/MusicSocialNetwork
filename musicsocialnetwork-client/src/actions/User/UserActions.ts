import { LoginData, RegisterData, Comment } from '../../models/User';

const apiUrl: string = 'http://localhost:57415/api/Users';

export async function authenticateAsync(loginData: LoginData) {
  try {
    const response = await fetch(apiUrl + '/authenticate', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(loginData)
    });
    return response.json();
  } catch (err) {
    console.log(err);
  }
}

export async function registerAsync(data: RegisterData) {
  const registerData: RegisterData = { ...data, role: 'User' };
  try {
    const response = await fetch(apiUrl + '/register', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(registerData)
    });
    return response.json();
  } catch (err) {
    console.log(err);
  }
}

export async function addComment(comment: Comment) {
  try {
    const response = await fetch(apiUrl + '/comment', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(comment)
    });
    return response.json();
  } catch (err) {
    console.log(err);
  }
}

export async function getComments(type: string, parentId: number) {
  try {
    const response = await fetch(apiUrl + '/comments?type=' + type + '&parentId=' + parentId, {
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