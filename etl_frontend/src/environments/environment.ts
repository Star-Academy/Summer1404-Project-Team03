const baseUrl = 'https://192.168.25.178:7252/api';

export const environment = {
  production: true,
  redirectUrl: 'http://localhost:4200/send-token-code',
  api: {
    auth: {
      login: `${baseUrl}/Auth/login`,
      token: `${baseUrl}/Auth/token`,
      //   register: `${baseUrl}/Auth/register`,
    },
    users: {
      //   me: `${baseUrl}/me`,
      //   all: `${baseUrl}/users`,
      //   byId: (id: string) => `${baseUrl}/users/${id}`
    }
  }
};
