import { Component } from '@angular/core';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { PasswordModule } from 'primeng/password';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [CardModule, InputTextModule, PasswordModule, ButtonModule, ReactiveFormsModule],
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.scss'
})
export class EditProfileComponent {
  editForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router) {
    this.editForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  onSubmit() {
    if (this.editForm.valid) {
      const newUsername = this.editForm.value.username;
      console.log('Saving username:', newUsername);

      // TODO: call backend API to update username
      this.router.navigate(['/profile/detail']);
    }
  }

  onCancel() {
    this.router.navigate(['/profile/detail']);
  }
}
