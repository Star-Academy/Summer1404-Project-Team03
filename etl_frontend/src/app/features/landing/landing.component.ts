import {Component} from '@angular/core';
import {Toolbar} from 'primeng/toolbar';
import {Button} from 'primeng/button';

@Component({
  selector: 'app-landing',
  imports: [
    Toolbar,
    Button,
  ],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss',
})
export class LandingComponent {
}
