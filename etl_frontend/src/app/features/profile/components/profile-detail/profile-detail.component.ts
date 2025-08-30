import {Component, computed, signal} from '@angular/core';

import {CardModule} from 'primeng/card';
import {AvatarModule} from 'primeng/avatar';
import {TagModule} from 'primeng/tag';
import {ButtonModule} from 'primeng/button';
import {DividerModule} from 'primeng/divider';
import {RouterModule} from "@angular/router";
import {UserStoreService} from "../../../../shared/stores/user-store.service";
import { ChangePasswordDirective } from '../../../../shared/directives/change-password/change-password.directive';
import {EditProfileComponent} from './components/edit-profile/edit-profile.component'

@Component({
  selector: 'app-profile-detail',
  imports: [
    CardModule,
    AvatarModule,
    TagModule,
    ButtonModule,
    DividerModule,
    ChangePasswordDirective,
    RouterModule,
    EditProfileComponent
  ],
  templateUrl: './profile-detail.component.html',
  styleUrl: './profile-detail.component.scss'
})
export class ProfileDetailComponent {
  public readonly user = computed(() => this.userStore.vm().user)
  public readonly userRole = computed(() => this.user().roles.map(r => r.name).join(', '));
  public readonly date = new Date().getUTCDate();

  public isEditProfileModal = signal<boolean>(false);

  constructor(private readonly userStore: UserStoreService) {
    if (this.user().firstName === '') this.userStore.loadUser();
  }

  public getSeverityForRole() {
    const roles = this.userRole().toLowerCase().split(',');

    switch (roles[0].trim()) {
      case 'sys_admin':
        return 'danger';
      case 'data_admin':
        return 'warning';
      case 'data_analist':
        return 'info';
      default:
        return 'secondary'
    }
  }

  public changeEditProfileModalStatus() {
    this.isEditProfileModal.update((currentValue) => !currentValue);
  }
}

