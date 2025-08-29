import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {Observable} from 'rxjs';
import {UserInfo} from '../../types/UserInfoType';

@Injectable({
  providedIn: 'root'
})

export class UsersService {
  private readonly usersApi = environment.api.users;

  constructor(private readonly http: HttpClient) {
  }

  public getUserInformation(): Observable<UserInfo> {
    return this.http.get<UserInfo>(this.usersApi.me);
  }
}
