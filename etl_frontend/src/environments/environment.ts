const baseUrl = 'https://192.168.25.178:7252/api';

export const environment = {
  production: true,
  redirectUrl: 'http://localhost:4200/send-token-code',
  api: {
    auth: {
      signIn: `${baseUrl}/auth/login`,
      token: `${baseUrl}/auth/token`,
      singOut: `${baseUrl}/auth/logout`,
    },
    prfile: {
      me: `${baseUrl}/profile/me`,
    },
    users: {}
  }
};
