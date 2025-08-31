import { Component, computed, input, output } from '@angular/core';
import { Dialog } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { PasswordModule } from 'primeng/password';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserStoreService } from '../../../../../../shared/stores/user-store.service';
import { UsersService } from '../../../../../../shared/services/user/users.service';
import { MessageService } from 'primeng/api';
import { InputTextModule } from 'primeng/inputtext';
import { UserUpdate } from '../../../../../../shared/types/UserType';
import { MessageModule } from 'primeng/message';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [Dialog, PasswordModule, ButtonModule, ReactiveFormsModule, InputTextModule, MessageModule, NgClass],
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.scss'
})
export class EditProfileComponent {
  public visible = input.required<boolean>();
  public close = output<void>();

  public readonly user = computed(() => this.userStore.vm().user);

  public exampleForm!: FormGroup;
  public formSubmitted = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly userStore: UserStoreService,
    private readonly userService: UsersService,
    private readonly messageService: MessageService
  ) {
    this.exampleForm = this.fb.group({
      firstName: [this.user().firstName, Validators.required],
      lastName: [this.user().lastName, Validators.required],
      email: [this.user().email, [Validators.required, Validators.email]],
    });
  }

  public onSubmit() {
    this.formSubmitted = true;
    if (!this.exampleForm.invalid) {
      console.log(this.exampleForm.value)
      this.exampleForm.markAllAsTouched();
      const newUserInfo: UserUpdate = this.exampleForm.value;

      this.userService.updateUserInformation(newUserInfo).subscribe({
        next: () => {
          this.userStore.loadUser();
          this.messageService.add({
            severity: 'success',
            summary: 'Profile successfully updated'
          });
          this.onClose();
        }
      });
    }
    this.formSubmitted = false;
  }

  public onClose(): void {
    this.close.emit();
  }

  public isInvalid(controlName: string) {
    const control = this.exampleForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }
}
