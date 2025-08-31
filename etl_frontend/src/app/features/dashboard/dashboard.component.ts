import {Component, computed, OnInit} from '@angular/core';
import {UserStoreService} from "../../shared/stores/user-store.service"

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  public readonly user = computed(() => this.userStore.vm().user);
  public readonly isLoading = computed(() => this.userStore.vm().isLoading);

  constructor(private userStore: UserStoreService) {}

  ngOnInit() {
    this.userStore.loadUser();
  }
}
