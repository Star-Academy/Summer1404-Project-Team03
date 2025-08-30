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

  users: User[] = [
    {
      id: '1',
      username: 'jdoe',
      email: 'jdoe@example.com',
      firstName: 'John',
      lastName: 'Doe',
      roles: []
    },
    {
      id: '2',
      username: 'asmith',
      email: 'asmith@example.com',
      firstName: 'Alice',
      lastName: 'Smith',
      roles: []
    },
    {
      id: '3',
      username: 'bjohnson',
      email: 'bjohnson@example.com',
      firstName: 'Bob',
      lastName: 'Johnson',
      roles: []
    },
    {
      id: '4',
      username: 'mmiller',
      email: 'mmiller@example.com',
      firstName: 'Mary',
      lastName: 'Miller',
      roles: []
    }
  ];

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
  public changeDeleteUserDialogStatus(userId?: string) {
    if (userId) {
      this.selectedUserToDelete.set(userId);
      this.isDeleteUserDialog.set(true);      
    } else {
      this.isDeleteUserDialog.set(false);
      this.selectedUserToDelete.set('');
    }
  }
}
