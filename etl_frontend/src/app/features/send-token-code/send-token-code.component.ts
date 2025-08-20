import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../shared/services/auth/auth.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-send-token-code',
  imports: [],
  templateUrl: './send-token-code.component.html',
  styleUrl: './send-token-code.component.scss'
})
export class SendTokenCodeComponent implements OnInit {
  constructor(
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly messageService: MessageService
  ) { }

  ngOnInit() {
    const tokenCode = this.getTokenCode();
    if (tokenCode) {
      this.sendToken(tokenCode);
    } else {
      this.router.navigate(['/landing']);
    }
  }

  getTokenCode(): string | undefined {
    return this.route.snapshot.queryParams['code'];
  }

  sendToken(tokenCode: string): void {
    this.authService.exchangeToken(tokenCode).subscribe({
      next: () => {
        this.showSigninSuccess();
        this.router.navigate(['/dashboard'])
      },
      error: () => {
        this.router.navigate(['/landing'])
        this.showSigninFail();
      }
    });
  }

  showSigninSuccess(): void {
    this.messageService.add({
      severity: 'success',
      summary: 'Signin Successfully',
      detail: 'Welcome to the Data Wave',
    });
  }

  showSigninFail(): void {
    this.messageService.add({
      severity: 'error',
      summary: 'Signin Failed',
      detail: 'An error occurred during Singin',
    });
  }
}
