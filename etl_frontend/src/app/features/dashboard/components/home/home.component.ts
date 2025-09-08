import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  public readonly menuItems: MenuItem[] = [
    {
      label: 'DataWave',
      items: [
        {
          label: 'Workflow History',
          icon: 'pi pi-file-plus',
          routerLink: ['/dashboard/workflows'],
          routerLinkActivate: true,
        },
        {
          label: 'Data Management',
          icon: 'pi pi-file-import',
          routerLink: ['/dashboard/files'],
        },
      ],
    },
  ];
}
