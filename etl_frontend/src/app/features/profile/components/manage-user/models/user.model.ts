export interface User {
    firstName: string;
    lastName: string;
    email: string;
    id: string;
    username: string;
    roles: UserRole[];
}

interface UserRole {
    id: string;
    name: string;
}

export interface UsersListState {
    users: User[];
    isLoading: boolean;
    error: string | null;
}

// interface User 