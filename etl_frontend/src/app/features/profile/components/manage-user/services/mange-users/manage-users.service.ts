import { Injectable } from '@angular/core';
import { environment } from '../../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../../models/user.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ManageUsersService {
  private readonly getAllusersApi: string = environment.api.admin.usersList;

  constructor(private readonly http: HttpClient) { }

  fetchUsers(): Observable<User> {
    return this.http.get<User>(this.getAllusersApi);
  }
}
