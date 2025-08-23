import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';

export interface SignInResponse {
  redirectUrl: string;
}

export interface TokenResponse {
  redirectUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly authApi = environment.api.auth;
  private readonly redirectUrl = environment.redirectUrl;

  constructor(private readonly http: HttpClient) {}

  public getSignInUrl(): Observable<SignInResponse> {
    return this.http.post<SignInResponse>(this.authApi.signIn, {
      redirectUrl: this.redirectUrl
    });
  }

  exchangeToken(code: string): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(this.authApi.token, {
      code,
      redirectUrl: this.redirectUrl
    });
  }

  signOut() {
    return this.http.post(this.authApi.singOut, {});
  }
}
