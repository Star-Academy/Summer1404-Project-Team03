import {Component, CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
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
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class LandingComponent {
}
