import {Component, OnInit} from '@angular/core';
import {ButtonModule} from 'primeng/button';
import {TableModule} from 'primeng/table'
import {CommonModule} from '@angular/common';
import {SkeletonModule} from "primeng/skeleton";
import {InputTextModule} from 'primeng/inputtext';
import {RippleModule} from 'primeng/ripple';
import {PaginatorModule} from 'primeng/paginator';
import { FormsModule } from '@angular/forms';

type Column = {
  field: string;
  header: string;
}

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
    FormsModule
  ],
  templateUrl: './manage-user.component.html',
  styleUrl: './manage-user.component.scss'
})
export class ManageUserComponent implements OnInit {
  public users: any[] = [];
  public loading: boolean = true;
  public clonedUsers: { [s: string]: any } = {};

  cols!: Column[];

  private readonly mockUsers = [
    {username: 'test1', email: '1', role: 'admin'},
    {username: 'test2', email: '2', role: 'admin'},
    {username: 'test3', email: '3', role: 'admin'},
    {username: 'test4', email: '4', role: 'admin'},
    {username: 'test5', email: '5', role: 'admin'},
    {username: 'test6', email: '6', role: 'admin'},
    {username: 'test7', email: '7', role: 'admin'}
  ];

  ngOnInit() {
    this.cols = [
      {field: 'username', header: 'Username'},
      {field: 'email', header: 'Email'},
      {field: 'role', header: 'Role'},
      {field: 'action', header: 'Action'},
    ];

    setTimeout(() => {
      this.users = this.mockUsers;
      this.loading = false;
    }, 2000);
  }

  onRowEditInit(user: any) {
    this.clonedUsers[user.username] = {...user};
  }

  onRowEditSave(user: any) {
    delete this.clonedUsers[user.username];
  }

  onRowEditCancel(user: any, index: number) {
    this.users[index] = this.clonedUsers[user.username];
    delete this.clonedUsers[user.username];
  }
}
