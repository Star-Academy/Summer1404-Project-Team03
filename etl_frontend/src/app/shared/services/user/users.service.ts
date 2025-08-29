import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { User } from '../../stores/user-store.service';

@Injectable({
  providedIn: 'root'
})

export class UsersService {
  private readonly usersApi = environment.api.users;

  constructor(private readonly http: HttpClient) { }

  public getUserInformation(): Observable<User> {
    return this.http.get<User>(this.usersApi.me);
  }
}
