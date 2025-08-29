export type UserInfo = {
  email: string;
  firstName: string;
  id: string;
  lastName: string | null;
  roles: [{id: string, name: string}];
  username: string;
}
