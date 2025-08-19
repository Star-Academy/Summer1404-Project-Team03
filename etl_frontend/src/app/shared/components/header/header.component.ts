import {Component} from '@angular/core';
import {Toolbar} from 'primeng/toolbar';

@Component({
  selector: 'app-header',
  imports: [
    Toolbar
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

}
