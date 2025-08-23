const baseUrl = 'https://192.168.25.194:7252/api';

export const environment = {
  production: false,
  api: {
    auth: {
      login: `${baseUrl}/Auth/login`,
      token: `${baseUrl}/Auth/token`,
    //   register: `${baseUrl}/Auth/register`,
    },
    profile: {
      me: `${baseUrl}/Profile/me`,
    },
    users: {
    //   me: `${baseUrl}/me`,
    //   all: `${baseUrl}/users`,
    //   byId: (id: string) => `${baseUrl}/users/${id}`
    }
  }
};