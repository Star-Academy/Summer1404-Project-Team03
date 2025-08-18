import {Component, CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';

import {ButtonModule} from 'primeng/button';
import {RouterOutlet} from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-root',
  imports: [ButtonModule, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppComponent {
}
