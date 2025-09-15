import {Component, CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import { ConfirmationService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import {Toast} from 'primeng/toast';

@Component({
  standalone: true,
  selector: 'app-root',
  imports: [RouterOutlet, Toast, ConfirmDialogModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  schemas: []
})
export class AppComponent {
}
