import { Component, input, output, OnInit } from '@angular/core';
import { Dialog } from 'primeng/dialog';
import { Button } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { CreateUserStore } from './stores/create-user/create-user-store.service';
import { NewUser, User } from '../../models/user.model';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-create-user-modal',
  standalone: true,
  imports: [Dialog, Button, SelectModule, InputTextModule, ReactiveFormsModule, NgIf],
  providers: [CreateUserStore],
  templateUrl: './create-user-modal.component.html',
  styleUrls: ['./create-user-modal.component.scss', './../../styles/shared-modal.component.scss']
})
export class CreateUserModalComponent implements OnInit {
  public visible = input.required<boolean>();
  public close = output<void>();
  public readonly vm;
  userForm!: FormGroup;


  constructor(
    private readonly createUserStore: CreateUserStore,
    private fb: FormBuilder
  ) {
    this.vm = this.createUserStore.vm;
  }

  ngOnInit(): void {
    this.userForm = this.fb.group({
      username: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  get f() {
    return this.userForm.controls;
  }

  public onClose(): void {
    this.userForm.reset();
    this.close.emit();
  }

  public onCreateUser(): void {
    this.userForm.markAllAsTouched();

    if (this.userForm.invalid) {
      return;
    }

    const newUser: NewUser = this.userForm.value;

    this.createUserStore.createUser({ user: newUser, onSuccess: this.onSuccessCreation.bind(this) });
  }

  onSuccessCreation(): void {
    this.close.emit()
  }
}
