import { Component } from '@angular/core';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { PasswordModule } from 'primeng/password';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-change-password',
  imports: [CardModule, InputTextModule, PasswordModule, ButtonModule, FormsModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss'
})
export class ChangePasswordComponent {
  currentPassword: string = '';
  newPassword: string = '';
  confirmPassword: string = '';

  changePassword() {
    if (this.newPassword !== this.confirmPassword) {
      alert("New password and confirm password do not match.");
      return;
    }
    // Call your API here
    console.log('Password changed:', this.newPassword);
  }
}
