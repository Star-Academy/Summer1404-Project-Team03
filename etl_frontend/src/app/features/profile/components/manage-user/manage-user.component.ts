import { Component, Signal, signal } from '@angular/core';
import { UserListStore } from './stores/user-list/user-list-store.service';
import { User, UsersListState } from './models/user.model';

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
  public selectedUserToDelete = signal<string>('');
  public selectedUserToEdit = signal<User>({ email: '', firstName: '', id: '', lastName: '', roles: [], username: '' });

  constructor(private usersListStore: UserListStore) {
    this.vm = this.usersListStore.vm;
    this.usersListStore.getUsers();
  }

  public changeCreateUserModalStatus() {
    this.isCreateUserModal.update((currentValue) => !currentValue);
  }

  public changeEditUserModalStatus(user?: User) {
    if (user) {
      this.selectedUserToEdit.set(user);
      this.isEditUserModal.set(true);
    } else {
      this.isEditUserModal.set(false);
      this.selectedUserToEdit.set({ email: '', firstName: '', id: '', lastName: '', roles: [], username: '' });
    }
  }

  public changeDeleteUserDialogStatus(userId?: string) {
    if (userId) {
      this.selectedUserToDelete.set(userId);
      this.isDeleteUserDialog.set(true);
    } else {
      this.isDeleteUserDialog.set(false);
      this.selectedUserToDelete.set('');
    }
  }

  public onRefetchUsers(): void {
    this.usersListStore.getUsers();
  }
}
