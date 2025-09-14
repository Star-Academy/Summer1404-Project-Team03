import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {Observable} from 'rxjs';
import { ChangePasswordResponse, updateUserInfoBody, updateUserInfoResponse, UserInfo } from '../../models/user.model';

@Injectable({
  providedIn: 'root'
})

export class UsersService {
  private readonly usersApi = environment.api.users;
  private readonly usersAuthApi = environment.api.auth;

  constructor(private readonly http: HttpClient) {}

  public getChnagePasswordUrl(): Observable<ChangePasswordResponse> {
    return this.http.get<ChangePasswordResponse>(this.usersApi.password);
  }

  public getUserInformation(): Observable<UserInfo> {
    return this.http.get<UserInfo>(this.usersAuthApi.me);
  }

  public updateUserInformation(newUserInfo: updateUserInfoBody): Observable<updateUserInfoResponse> {
    return this.http.put<updateUserInfoResponse>(this.usersApi.me, newUserInfo);
  }
}
