import {Component, CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import {Toolbar} from 'primeng/toolbar';
import {Button} from 'primeng/button';
import {SignInDirective} from '../../shared/directives/sign-in.directive';

@Component({
  selector: 'app-landing',
  imports: [
    Toolbar,
    Button,
    SignInDirective,
  ],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class LandingComponent {
}
