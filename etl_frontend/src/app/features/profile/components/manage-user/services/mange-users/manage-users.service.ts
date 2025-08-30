import { Injectable } from '@angular/core';
import { environment } from '../../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { NewUser, User } from '../../models/user.model';
import { Observable, tap } from 'rxjs';

@Injectable()
export class ManageUsersService {
  private readonly getAllusersApi: string = environment.api.admin.usersList;
  private readonly createUserApi: string = environment.api.admin.createUser;

  constructor(private readonly http: HttpClient) { }

  fetchUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.getAllusersApi);
  }

  createUser(newUser: NewUser): Observable<NewUser> {
    return this.http.post<NewUser>(this.createUserApi, newUser)
  }
}
