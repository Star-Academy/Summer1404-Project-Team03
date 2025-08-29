export interface User {
    firstName: string;
    lastName: string;
    email: string;
    id: string;
    username: string;
}

export interface UsersListState {
    users: User[];
    isLoading: boolean;
    error: string | null;
}

// interface User 