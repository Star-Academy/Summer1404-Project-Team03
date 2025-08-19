import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly authApi = environment.api.auth;
  private readonly redirectUrl = "http://localhost:4200/dashboard";
  constructor(private readonly http: HttpClient) {}

  public getSignInUrl() {
    return this.http.post<{redirectUrl: string}>(this.authApi.login, {redirectUrl: this.redirectUrl})
  }
}
