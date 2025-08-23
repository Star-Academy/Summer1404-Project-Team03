import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private readonly usersApi = environment.api.users;

  constructor(private readonly http: HttpClient) { }

  public getUserInformation() {
    return this.http.get(this.usersApi.me);
  }
}
