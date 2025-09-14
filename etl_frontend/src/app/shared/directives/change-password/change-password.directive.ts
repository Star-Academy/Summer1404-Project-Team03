import { ChangeDetectorRef, Directive, effect, Host, HostListener, signal } from '@angular/core';
import { UsersService } from '../../services/user/users.service';
import { MessageService } from 'primeng/api';
import { Button } from 'primeng/button';
import { take } from 'rxjs';

@Directive({
  selector: '[appChangePassword]'
})
export class ChangePasswordDirective {
  private readonly isLoading = signal(false);

  constructor(
    @Host() private readonly hostButton: Button,
    private readonly usersService: UsersService,
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

    this.usersService.getChangePasswordUrl().pipe(take(1)).subscribe({
      next: ({ changePasswordUrl }) => {
        this.isLoading.set(false);
        this.redirect(changePasswordUrl);
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
      summary: 'Request failed.',
      detail: 'An error occurred during change password. Please try again later.',
    });
  }
}
