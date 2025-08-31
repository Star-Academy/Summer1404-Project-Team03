import { Component, effect, input, OnInit, output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { NgIf } from '@angular/common';

import { EditUserStore } from './stores/edit-user/edit-user-store.service';
import { EditUser, User, UserRole } from '../../models/user.model';

@Component({
  selector: 'app-edit-user-modal',
  standalone: true,
  imports: [
    NgIf,
    ReactiveFormsModule, 
    ButtonModule, 
    DialogModule, 
    DropdownModule, 
    InputTextModule
  ],
  templateUrl: './edit-user-modal.component.html',
  styleUrls: ['./edit-user-modal.component.scss', './../../styles/shared-modal.component.scss']
})
export class EditUserModalComponent implements OnInit {
  public visible = input.required<boolean>();
  public userInfo = input.required<User>();
  public close = output<void>();

  public readonly vm;
  public userForm!: FormGroup;
  
  // public roles: UserRole[] = [
  //   { id: 'Admin', name: 'ADMIN' },
  //   { id: 'User', name: 'USER' },
  //   { id: 'Viewer', name: 'VIEWER' }
  // ];

  constructor(
    private readonly editUserStore: EditUserStore,
    private readonly fb: FormBuilder
  ) {
    this.vm = this.editUserStore.vm;

    // effect(() => {
    //   const user = this.userInfo();
    //   if (user && this.userForm) {
    //     this.userForm.patchValue({
    //       email: user.email,
    //       firstName: user.firstName,
    //       lastName: user.lastName,
    //       role: this.roles.find(r => r.name === user.roles.name) || null
    //     });
    //   }
    // });
  }

  ngOnInit(): void {
    this.userForm = this.fb.group({
      email: [this.userInfo().email, [Validators.required, Validators.email]],
      firstName: [this.userInfo().firstName, [Validators.required]],
      lastName: [this.userInfo().lastName, [Validators.required]],
    });
  }

  get f() {
    return this.userForm.controls;
  }

  public onClose(): void {
    this.userForm.reset();
    this.close.emit();
  }

  public onEditUser(): void {
    this.userForm.markAllAsTouched();

    if (this.userForm.invalid) {
      return;
    }

    const formValue: EditUser = this.userForm.value;

    this.editUserStore.editUser({ 
      userNewInfo: formValue, 
      userId: this.userInfo().id,
      onSuccess: this.onSuccessCreation.bind(this) 
    });
  }

  onSuccessCreation(): void {
    this.onClose();
  }
}