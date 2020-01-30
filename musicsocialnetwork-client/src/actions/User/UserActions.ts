import { LoginData, RegisterData } from '../../models/User';

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