import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../shared/services/auth/auth.service';

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
      next: () => this.router.navigate(['/dashboard']),
      error: () => this.router.navigate(['/landing'])
    });
  }
}
