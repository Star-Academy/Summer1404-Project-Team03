export type SignInResponse = {
  signInUrl: string;
}

export type SendTokenCodeResponse = {
  redirectUrl: string;
}

export type SendTokenCodeBody = {
  code: string;
  redirectUrl: string;
}