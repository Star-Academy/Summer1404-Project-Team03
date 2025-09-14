import { Component, effect, input, OnInit, output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { NgIf } from '@angular/common';

import { EditUserStore } from './stores/edit-user/edit-user-store.service';
import { EditUser, User, UserRole } from '../../models/user.model';
import { MessageService } from 'primeng/api';

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
  public userUpdated = output<void>();

  public readonly vm;
  public userForm!: FormGroup;

  constructor(
    private readonly editUserStore: EditUserStore,
    private readonly fb: FormBuilder,
    private readonly messageService: MessageService
  ) {
    this.vm = this.editUserStore.vm;
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
    this.userUpdated.emit();
    this.messageService.add({
      severity: 'success',
      summary: 'User Updated',
      detail: `User ${this.userForm.value['firstName']} was updated successfully.`,
      life: 3000
    });
    this.onClose();
  }
}