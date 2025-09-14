import {Component} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
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
