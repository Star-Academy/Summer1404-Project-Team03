import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { SendTokenCodeBody, SendTokenCodeResponse, SignInResponse } from '../../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly authApi = environment.api.auth;
  private readonly redirectUrl = environment.redirectUrl;

  constructor(private readonly http: HttpClient) { }

  public getSignInUrl(): Observable<SignInResponse> {
    return this.http.post<SignInResponse>(this.authApi.signIn, {
      redirectUrl: this.redirectUrl
    });
  }

  sendTokenCode(code: string): Observable<SendTokenCodeResponse> {
    const body: SendTokenCodeBody = { code: code, redirectUrl: this.redirectUrl }
    return this.http.post<SendTokenCodeResponse>(this.authApi.token, body);
  }

  signOut() {
    return this.http.post(this.authApi.signOut, {});
  }
}
