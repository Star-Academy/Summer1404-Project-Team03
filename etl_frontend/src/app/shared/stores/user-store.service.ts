import {Injectable} from '@angular/core';
import {ComponentStore} from "@ngrx/component-store";
import {finalize, tap} from 'rxjs';
import {UsersService} from "../services/user/users.service"
import {UserInfo} from '../types/UserType';

type UserStore = {
  user: UserInfo;
  isLoading: boolean;
  isSysAdmin: boolean;
}

const initialUser = {
  user: {
    email: '',
    firstName: '',
    id: '',
    lastName: '',
    roles: [],
    username: '',
  },
  isLoading: false,
  isSysAdmin: false,
}

@Injectable({
  providedIn: 'root'
})
export class UserStoreService extends ComponentStore<UserStore> {
  constructor(private readonly http: UsersService) {
    super(initialUser);
  }

  private readonly user = this.selectSignal((state) => state.user);
  private readonly isLoading = this.selectSignal((state) => state.isLoading);
  private readonly isSysAdmin = this.selectSignal((state) => state.isSysAdmin);

  private readonly setUser = this.updater((state, user: UserInfo) => ({...state, user}));
  private readonly setLoading = this.updater((state, value: boolean) => ({
    ...state,
    isLoading: value
  }));
  private readonly setSysAdmin = this.updater((state, value: boolean) => ({
    ...state,
    isSysAdmin: value
  }));

  public readonly vm = this.selectSignal(
    this.user,
    this.isLoading,
    this.isSysAdmin,
    (user, isLoading, isSysAdmin) => ({user, isLoading, isSysAdmin})
  );

  public loadUser(): void {
    this.setLoading(true);

    this.http.getUserInformation().pipe(
      tap({
        next: (user: UserInfo) => {
          const isSysAdmin = user.roles.some(role => role.name === 'sys_admin')
          this.setUser(user)
          this.setSysAdmin(isSysAdmin)
        },
      }),
      finalize(() => this.setLoading(false)),
    ).subscribe();
  }

  public clearUser(): void {
    this.patchState(initialUser);
  }
}
