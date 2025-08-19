import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {AuthService} from '../../shared/services/auth/auth.service';

@Component({
  selector: 'app-send-token-code',
  imports: [],
  templateUrl: './send-token-code.component.html',
  styleUrl: './send-token-code.component.scss'
})
export class SendTokenCodeComponent implements OnInit {
  constructor(private readonly route: ActivatedRoute,
              private readonly authService: AuthService,
              private readonly router: Router,
  ) {}

  ngOnInit() {
    const tokenCode = this.route.snapshot.queryParams['code'];
  setTimeout(()=> {
    console.log(tokenCode);
    if (tokenCode) {
      this.authService.sendToken(tokenCode).subscribe({
        next: () => this.router.navigate(['/dashboard']),
        error: () => this.router.navigate(['/landing'])
      });
    } else {
      this.router.navigate(['/landing']);
    }
  }, 5000)
  }
}
