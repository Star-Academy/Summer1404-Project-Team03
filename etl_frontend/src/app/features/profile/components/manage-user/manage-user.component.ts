import { Component, OnInit, Signal, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table'
import { CommonModule } from '@angular/common';
import { SkeletonModule } from "primeng/skeleton";
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { PaginatorModule } from 'primeng/paginator';
import { FormsModule } from '@angular/forms';

import { CreateUserModalComponent } from './components/create-user-modal/create-user-modal.component';
import { EditUserModalComponent } from './components/edit-user-modal/edit-user-modal.component';
import { DeleteUserDialogComponent } from './components/delete-user-dialog/delete-user-dialog.component';
import { UserListStore } from './stores/user-list/user-list-store.service';
import { UsersListState } from './models/user.model';

@Component({
  selector: 'app-manage-user',
  standalone: true,
  imports: [
    ButtonModule,
    TableModule,
    CommonModule,
    SkeletonModule,
    InputTextModule,
    RippleModule,
    PaginatorModule,
    FormsModule,
    CreateUserModalComponent,
    EditUserModalComponent,
    DeleteUserDialogComponent
  ],
  providers: [
    UserListStore
  ],
  templateUrl: './manage-user.component.html',
  styleUrl: './manage-user.component.scss'
})
export class ManageUserComponent implements OnInit {
  public readonly vm: Signal<UsersListState>;

  public isCreateUserModal = signal<boolean>(false);
  public isEditUserModal = signal<boolean>(false);
  public isDeleteUserDialog = signal<boolean>(false);

  constructor(private usersListStore: UserListStore) {
    this.vm = this.usersListStore.vm;
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

  ngOnInit() {}
}
