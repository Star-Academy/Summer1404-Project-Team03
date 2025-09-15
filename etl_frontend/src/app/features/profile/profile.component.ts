import {Component, computed} from '@angular/core';
import {UserStoreService} from '../../shared/stores/user-store.service';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent {
  public readonly user = computed(() => this.userStore.vm().user);

  constructor(private readonly userStore: UserStoreService) {
    if (this.user().firstName === '') this.userStore.loadUser();
  }
}
