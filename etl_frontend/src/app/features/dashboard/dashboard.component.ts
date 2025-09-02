import {Component, computed, OnInit} from '@angular/core';
import {UserStoreService} from "../../shared/stores/user-store.service"
import { MenuItem } from 'primeng/api';


@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  public readonly user = computed(() => this.userStore.vm().user);
  public readonly isLoading = computed(() => this.userStore.vm().isLoading);

  public readonly menuItems: MenuItem[] = [
    {
      label: 'DataWave',
      items: [
        {
          label: 'Workflow History',
          icon: 'pi pi-file-plus',
          routerLink: '/profile',
        },
        {
          label: 'Data Management',
          icon: 'pi pi-file-import',
          routerLink: '/profile',
        }
      ]
    }
  ];

  constructor(private userStore: UserStoreService) {}

  ngOnInit() {
    this.userStore.loadUser();
  }
}
