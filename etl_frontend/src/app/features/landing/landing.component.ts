import {Component, CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import {Button} from 'primeng/button';
import {HeaderComponent} from '../../shared/components/header/header.component';
import {SignInDirective} from '../../shared/directives/sign-in/sign-in.directive';

@Component({
  selector: 'app-landing',
  imports: [
    Button,
    HeaderComponent,
    SignInDirective,
  ],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class LandingComponent {
}
