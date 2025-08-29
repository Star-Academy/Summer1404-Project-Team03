import {Component, computed, effect, OnInit, signal} from '@angular/core';
import {User, UserStoreService} from "../../shared/stores/user-store.service"

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  public readonly user = computed(() => this.userStore.vm().user);

  constructor(private userStore: UserStoreService) {}

  ngOnInit() {
    this.userStore.loadUser();
  }
}
