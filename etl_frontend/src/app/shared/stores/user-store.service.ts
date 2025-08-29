import {Injectable} from '@angular/core';
import {ComponentStore} from "@ngrx/component-store";
import {finalize, tap} from 'rxjs';
import {UsersService} from "../services/user/users.service";
import {UserInfo} from '../types/UserInfoType';

type UserStore = {
  user: UserInfo;
  isLoading: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class UserStoreService extends ComponentStore<UserStore> {
  constructor(private readonly http: UsersService) {
    super({
      user: {
        email: '',
        firstName: '',
        id: '',
        lastName: null,
        roles: [{id: '', name: ''}],
        username: '',
      }, isLoading: false
    });
  }

  private readonly user = this.selectSignal((state) => state.user);
  private readonly isLoading = this.selectSignal((state) => state.isLoading);

  public readonly vm = this.selectSignal(
    this.user,
    this.isLoading,
    (user, isLoading) => ({user, isLoading})
  );

  readonly setUser = this.updater((state, user: UserInfo) => ({...state, user}));

  public loadUser(): void {
    this.patchState({isLoading: true});

    this.http.getUserInformation().pipe(
      tap({
        next: (user: UserInfo) => {
          this.setUser(user)
        },
      }),
      finalize(() => this.patchState({isLoading: false}))
    ).subscribe();
  }
}
