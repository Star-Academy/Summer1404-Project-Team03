import {Component, computed, effect} from '@angular/core';
import {UserStoreService} from '../../shared/stores/user-store.service';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent {
  public readonly user = computed(() => this.userStore.vm().user);
  public readonly isSysAdmin = computed(() =>
    this.userStore.vm().user.roles.some(role => role.name === 'sys_admin')
  );

  constructor(private readonly userStore: UserStoreService) {
    if (this.user().firstName === '') this.userStore.loadUser();
  }
}
