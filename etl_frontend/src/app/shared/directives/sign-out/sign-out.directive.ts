import { Directive, effect, Host, HostListener, signal } from '@angular/core';
import { Button } from 'primeng/button';
import { AuthService } from '../../services/auth/auth.service';
import { MessageService } from 'primeng/api';
import { take } from 'rxjs';
import { Router } from '@angular/router';

@Directive({
  selector: '[appSignOut]',
  standalone: true,
})
export class SignOutDirective {
  private readonly isLoading = signal(false);

  constructor(
    @Host() private readonly hostButton: Button,
    private readonly authService: AuthService,
    private readonly messageService: MessageService,
    private readonly router: Router
  ) {
    effect(() => {
      this.hostButton.loading = this.isLoading();
    });
  }

  @HostListener('click')
  handleClick(): void {
    if (this.isLoading()) return;

    this.isLoading.set(true);

    this.authService.signOut().pipe(take(1)).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.showSuccess();
        setTimeout(() => {
          this.router.navigateByUrl('landing');
        }, 1000)
      },
      error: () => {
        this.isLoading.set(false);
      }
    })
  }

  private showSuccess(): void {
    this.messageService.add({
      severity: 'info',
      summary: 'You are signedout.',
    });
  }
}
