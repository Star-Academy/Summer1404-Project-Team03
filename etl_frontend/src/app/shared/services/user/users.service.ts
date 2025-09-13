import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {Observable} from 'rxjs';
import {UserInfo} from '../../types/UserType';
import { UserUpdate } from '../../types/UserType';

export type ChangePasswordResponse = {
  changePasswordUrl: string;
}
@Injectable({
  providedIn: 'root'
})

export class UsersService {
  private readonly usersApi = environment.api.users;
  private readonly usersAuthApi = environment.api.auth;

  constructor(private readonly http: HttpClient) {}

  public changePassword(): Observable<ChangePasswordResponse> {
    return this.http.get<ChangePasswordResponse>(this.usersApi.password);
  }

  public getUserInformation(): Observable<UserInfo> {
    return this.http.get<UserInfo>(this.usersAuthApi.me);
  }

  public updateUserInformation(newUserInfo: UserUpdate) {
    return this.http.put<UserUpdate>(this.usersApi.me, newUserInfo);
  }
}
