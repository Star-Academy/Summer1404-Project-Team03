const baseUrl = 'http://localhost:5000/api';

export const environment = {
  production: false,
  redirectUrl: `http://localhost:4200/send-token-code`,
  api: {
    auth: {
      signIn: `${baseUrl}/auth/login`,
      token: `${baseUrl}/auth/token`,
      singOut: `${baseUrl}/auth/logout`,
    },
    users: {
      me: `${baseUrl}/auth/me`,
    },
    admin: {
      usersList: `${baseUrl}/admin/users`
    }
  }
};
