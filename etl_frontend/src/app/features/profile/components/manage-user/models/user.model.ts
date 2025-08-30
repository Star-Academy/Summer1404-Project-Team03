interface baseUser {
    firstName: string;
    lastName: string;
    email: string;
    username: string;
}
export interface User extends baseUser{
    id: string;
    roles: UserRole[];
}

export interface NewUser extends baseUser{
    password: string;
}

interface UserRole {
    id: string;
    name: string;
}

export interface UsersListState extends State {
    users: User[];
}

export interface NewUserState extends State {
    user: NewUser;
}

interface State {
    isLoading: boolean;
    error: string | null;
}

// interface User 