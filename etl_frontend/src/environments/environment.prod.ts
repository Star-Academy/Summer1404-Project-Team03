const baseUrl = 'http://localhost:5000/api';

export const environment = {
  production: true,
  redirectUrl: `http://localhost:4200/send-token-code`,
  api: {
    // ===================== AUTH & PROFILE =====================
    auth: {
      signIn: `${baseUrl}/auth/login`, // POST
      token: `${baseUrl}/auth/token`, // POST
      signOut: `${baseUrl}/auth/logout`, // POST
      me: `${baseUrl}/auth/me`, // GET, PUT
      password: `${baseUrl}/auth/change-password-url`, // GET
    },

    // ===================== ADMIN =====================
    admin: {
      users: `${baseUrl}/admin/users`, // GET, POST
      user: (userId: string | number) => `${baseUrl}/admin/users/${userId}`, // GET, PUT, DELETE
      userRoles: (userId: string | number) => `${baseUrl}/admin/users/${userId}/roles`, // PUT
      roles: `${baseUrl}/admin/roles`, // GET
    },

    // ===================== FILES =====================
    files: {
      root: `${baseUrl}/files`, // GET
      upload: `${baseUrl}/files/stage-many`, // POST
      details: (id: string | number) => `${baseUrl}/files/${id}`, // Base for file-specific actions
      delete: (id: string | number) => `${baseUrl}/files/${id}`, // DELETE
      load: (id: string | number) => `${baseUrl}/files/${id}/load`, // POST
      registerAndLoad: (id: string | number) => `${baseUrl}/files/${id}/register-and-load`, // POST
      schema: {
        preview: (id: string | number) => `${baseUrl}/files/${id}/schema/preview`, // GET
        register: (id: string | number) => `${baseUrl}/files/${id}/schema/register`, // POST
      },
    },

    // ===================== TABLES & COLUMNS =====================
    tables: {
      list: `${baseUrl}/tables`, // GET
      details: (schemaId: string | number) => `${baseUrl}/tables/${schemaId}/details`, // GET
      rows: (schemaId: number, offset: number, limit: number) =>
        `${baseUrl}/tables/${schemaId}/rows?offset${offset}$limit=${limit}`,
      count: (schemaId: string | number) => `${baseUrl}/tables/${schemaId}/count`, // GET
      rename: (schemaId: string | number) => `${baseUrl}/tables/${schemaId}/rename`, // POST
      delete: (schemaId: string | number) => `${baseUrl}/tables/${schemaId}`, // DELETE
      columns: {
        list: (schemaId: string | number) => `${baseUrl}/tables/${schemaId}/columns`, // GET
        delete: (schemaId: string | number) => `${baseUrl}/tables/${schemaId}/columns`, // DELETE
        rename: (schemaId: string | number, columnId: string | number) =>
          `${baseUrl}/tables/${schemaId}/columns/${columnId}/rename`, // POST
        types: `${baseUrl}/types/columns`,
      },
    },

    // ===================== TABLES & COLUMNS =====================
    workflows: {
      list: `${baseUrl}/workflows`, // GET, POST
      update: (id: string) => `${baseUrl}/workflows${id}`, // PUT
      delete: (id: string) => `${baseUrl}/workflows${id}`, // DELETE
      item: (id: string) => `${baseUrl}/workflows${id}`, // GET
    },
  },
};