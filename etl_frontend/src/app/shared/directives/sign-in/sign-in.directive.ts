import {ChangeDetectorRef, Directive, effect, Host, HostListener, signal } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
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
    private readonly messageService: MessageService,
    private readonly cdr: ChangeDetectorRef
  ) {
    effect(() => {
      this.hostButton.loading = this.isLoading();
      this.cdr.detectChanges();
    });
  }

  @HostListener('click')
  handleClick(): void {
    if (this.isLoading()) return;

    this.isLoading.set(true);

    this.authService.getSignInUrl().pipe(take(1)).subscribe({
      next: ({ redirectUrl }) => {
        this.isLoading.set(false);
        this.redirect(redirectUrl);
      },
      error: () => {
        this.isLoading.set(false);
        this.showError();
      },
    });
  }

  private redirect(url: string): void {
    window.location.href = url;
  }

  private showError(): void {
    this.messageService.add({
      severity: 'error',
      summary: 'Signin Failed',
      detail: 'An error occurred during Singin. Please try again later.',
    });
  }
}
