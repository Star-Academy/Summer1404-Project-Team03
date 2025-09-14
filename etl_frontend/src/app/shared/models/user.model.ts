export type UserInfo = {
    email: string;
    firstName: string;
    id: string;
    lastName: string;
    roles: { id: string, name: string }[];
    username: string;
}

export type updateUserInfoBody = {
    email: string;
    firstName: string;
    lastName: string;
}

export type updateUserInfoResponse = {
    id: string;
    username: string;
    email: string;
    firstName: string;
    lastName: string;
}

export type ChangePasswordResponse = {
    changePasswordUrl: string;
}