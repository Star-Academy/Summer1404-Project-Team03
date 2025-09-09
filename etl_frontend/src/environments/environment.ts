const baseUrl = 'http://localhost:5000/api';

export const environment = {
  production: false,
  redirectUrl: `http://localhost:4200/send-token-code`,
  api: {
    // ===================== AUTH =====================
    auth: {
      signIn: `${baseUrl}/auth/login`,
      token: `${baseUrl}/auth/token`,
      signOut: `${baseUrl}/auth/logout`,
      me: `${baseUrl}/auth/me`,
    },

    // ===================== USERS / PROFILE =====================
    users: {
      me: `${baseUrl}/users/me`, // GET, PUT
      password: `${baseUrl}/password/change-password-url`, // GET
    },

    // ===================== ADMIN =====================
    admin: {
      users: `${baseUrl}/admin/users`, // GET, POST
      user: (userId: string | number) => `${baseUrl}/admin/users/${userId}`, // GET, PUT, DELETE
      userRoles: (userId: string | number) => `${baseUrl}/admin/users/${userId}/roles`, // PUT
      roles: `${baseUrl}/admin/roles`, // GET
    },

    // ===================== TABLES =====================
    tables: {
      list: `${baseUrl}/tables`, // GET
      rename: (schemaId: number) => `${baseUrl}/tables/${schemaId}/rename`, // POST
      delete: (schemaId: string) => `${baseUrl}/tables/${schemaId}`, // DELETE
    },

    // ===================== COLUMNS =====================
    columns: {
      list: (schemaId: number) => `${baseUrl}/tables/${schemaId}/columns`, // GET
      delete: (schemaId: number) => `${baseUrl}/tables/${schemaId}/columns`, // DELETE
      rename: (schemaId: number, columnId: number) =>
        `${baseUrl}/tables/${schemaId}/columns/${columnId}/rename`, // POST
    },

    // ===================== FILES =====================
    files: {
      root: `${baseUrl}/files`, // GET
      delete: (id: number) => `${baseUrl}/files/${id}`, // GET
      upload: `${baseUrl}/files/stage-many`, // POST
      previewSchema: (id: string | number) => `${baseUrl}/files/${id}/schema/preview`, // GET
      registerSchema: (id: string | number) => `${baseUrl}/files/${id}/schema/register`, // POST
      load: (id: string | number) => `${baseUrl}/files/${id}/load`, // POST
    },
  },
};
