import {Directive, effect, Host, HostListener, signal} from '@angular/core';
import {AuthService} from '../services/auth/auth.service';
import {MessageService} from 'primeng/api';
import {Button} from 'primeng/button';

@Directive({
  selector: '[appSignIn]',
  standalone: true
})
export class SignInDirective {
  constructor(
    @Host() private myComponent: Button,
    private readonly AuthService: AuthService,
    private readonly messageService: MessageService
  ) {
    effect(() => {
      this.myComponent.loading = this.isLoading();
    });
  }

  public isLoading = signal<boolean>(false);

  @HostListener('click')
  onSignIn() {
    if (this.isLoading()) return;

    this.isLoading.set(true);
    this.myComponent.loading = true;
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
