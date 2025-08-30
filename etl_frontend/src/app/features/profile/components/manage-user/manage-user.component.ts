import { Component, Signal, signal } from '@angular/core';
import { UserListStore } from './stores/user-list/user-list-store.service';
import { UsersListState } from './models/user.model';

@Component({
  selector: 'app-manage-user',
  standalone: false,
  templateUrl: './manage-user.component.html',
  styleUrl: './manage-user.component.scss'
})
export class ManageUserComponent {
  public readonly vm: Signal<UsersListState>;

  public isCreateUserModal = signal<boolean>(false);
  public isEditUserModal = signal<boolean>(false);
  public isDeleteUserDialog = signal<boolean>(false);

  constructor(private usersListStore: UserListStore) {
    this.vm = this.usersListStore.vm;
    this.usersListStore.getUsers();
  }

  public changeCreateUserModalStatus() {
    this.isCreateUserModal.update((currentValue) => !currentValue);
  }
  public changeEditUserModalStatus() {
    this.isEditUserModal.update((currentValue) => !currentValue);
  }
  public changeDeleteUserDialogStatus() {
    this.isDeleteUserDialog.update((currentValue) => !currentValue);
  }
}
