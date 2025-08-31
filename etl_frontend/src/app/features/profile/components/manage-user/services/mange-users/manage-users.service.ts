import { Injectable } from '@angular/core';
import { environment } from '../../../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { EditUser, NewUser, User } from '../../models/user.model';
import { Observable } from 'rxjs';

@Injectable()
export class ManageUsersService {
  private readonly usersApi: string = environment.api.admin.users;

  constructor(private readonly http: HttpClient) { }

  fetchUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.usersApi);
  }

  createUser(newUser: NewUser): Observable<NewUser> {
    return this.http.post<NewUser>(this.usersApi, newUser)
  }

  deleteUser(userId: string): Observable<any> {
    return this.http.delete<any>(`${this.usersApi}/${userId}`);
  }

  editUser(user: EditUser, userId: string): Observable<any> {
    return this.http.put<any>(`${this.usersApi}/${userId}`, user);
  }
}
