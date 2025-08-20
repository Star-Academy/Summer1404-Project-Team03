import { Directive, effect, Host, HostListener, signal } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';
import { MessageService } from 'primeng/api';
import { Button } from 'primeng/button';
import { take } from 'rxjs/operators';

@Directive({
  selector: '[appSignIn]',
  standalone: true,
})
export class SignInDirective {
  private readonly isLoading = signal(false);

  constructor(
    @Host() private readonly hostButton: Button,
    private readonly authService: AuthService,
    private readonly messageService: MessageService
  ) {
    effect(() => {
      this.hostButton.loading = this.isLoading();
    });
  }

  @HostListener('click')
  handleClick(): void {
    if (this.isLoading()) return;

    this.setLoading(true);

    this.authService.getSignInUrl().pipe(take(1)).subscribe({
      next: ({ redirectUrl }) => {
        this.setLoading(false);
        this.redirect(redirectUrl);
      },
      error: () => {
        this.setLoading(false);
        this.showError();
      },
    });
  }

  private setLoading(state: boolean): void {
    this.isLoading.set(state);
  }

  private redirect(url: string): void {
    window.location.href = url;
  }

  private showError(): void {
    this.messageService.add({
      severity: 'error',
      summary: 'Login Failed',
      detail: 'An error occurred during sign-in. Please try again later.',
    });
  }
}
