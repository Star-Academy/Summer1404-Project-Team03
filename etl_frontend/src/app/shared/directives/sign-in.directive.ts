import {Directive, HostBinding, HostListener, signal} from '@angular/core';
import {AuthService} from '../services/auth/auth.service';
import {MessageService} from 'primeng/api';
import {Router} from '@angular/router';

@Directive({
  selector: '[appSignIn]',
  standalone: true
})
export class SignInDirective {
  constructor(
    private readonly AuthService: AuthService,
    private readonly messageService: MessageService,
    private readonly router: Router) {
  }

  public isLoading = signal<boolean>(false);

  @HostBinding('loading')
  get loading() {
    return this.isLoading();
  }

  // ✅ disable button when loading
  @HostBinding('disabled')
  get disabled() {
    return this.isLoading();
  }

  @HostListener('click')
  onSignIn() {
    if (this.isLoading()) return;

    this.isLoading.set(true);
    this.AuthService.getSignInUrl().subscribe({
      next: (res: { redirectUrl: string }) => {
        this.isLoading.set(false);
        window.location.href = res.redirectUrl;
      },
      error: () => {
        this.isLoading.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Login Failed',
          detail: 'An error occurred during sign-in. Please try again later.'
        })
      },
    })
  }
}
