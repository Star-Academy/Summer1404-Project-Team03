import {Component, computed, OnInit} from '@angular/core';
import {UserStoreService} from "../../shared/stores/user-store.service"
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  public readonly user = computed(() => this.userStore.vm().user);
  public readonly isLoading = computed(() => this.userStore.vm().isLoading);

  constructor(private userStore: UserStoreService) {}
}
