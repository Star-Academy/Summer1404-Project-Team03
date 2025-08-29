import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {Observable} from 'rxjs';
import {UserInfo} from '../../types/UserInfoType';

export type ChangePasswordResponse = {
  changePasswordUrl: string;
}
@Injectable({
  providedIn: 'root'
})

export class UsersService {
  private readonly usersApi = environment.api.users;

  constructor(private readonly http: HttpClient) {
  }

  public changePassword(): Observable<ChangePasswordResponse> {
    return this.http.get<ChangePasswordResponse>(this.usersApi.password);
  }

  public getUserInformation(): Observable<UserInfo> {
    return this.http.get<UserInfo>(this.usersApi.me);
  }
}
