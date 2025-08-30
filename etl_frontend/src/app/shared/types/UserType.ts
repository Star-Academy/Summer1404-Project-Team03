export type UserInfo = {
  email: string;
  firstName: string;
  id: string;
  lastName: string;
  roles: [{id: string, name: string}];
  username: string;
}

export type UserUpdate = {
  email: string;
  firstName: string;
  lastName: string ;
}
