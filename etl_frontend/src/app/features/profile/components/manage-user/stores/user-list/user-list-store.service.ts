import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { UsersListState } from '../../models/user.model';

const initialUser: UsersListState = {
  users: [],
  isLoading: true,
  error: null
}

@Injectable()
export class UserListStore extends ComponentStore<UsersListState> {

  constructor() {
    super(initialUser);
  }

  public readonly vm = this.state();
}
