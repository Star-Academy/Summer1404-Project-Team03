import { Component } from '@angular/core';

import { CardModule } from 'primeng/card';
import { AvatarModule } from 'primeng/avatar';
import { TagModule } from 'primeng/tag';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { Image } from 'primeng/image';
import { DatePipe } from '@angular/common';

interface User {
  name: string;
  username: string;
  email: string;
  imageUrl: string;
  role: 'Admin' | 'Editor' | 'User'; // Use literal types for roles
  memberSince: Date;
}

@Component({
  selector: 'app-profile-detail',
  imports: [CardModule,
    AvatarModule,
    TagModule,
    ButtonModule,
    Image,
    DividerModule,
    DatePipe],
  templateUrl: './profile-detail.component.html',
  styleUrl: './profile-detail.component.scss'
})
export class ProfileDetailComponent {
  user: User = {
    name: 'Eleanor Vance',
    username: 'evance',
    email: 'eleanor.vance@example.com',
    // imageUrl: 'https://primefaces.org/cdn/primeng/images/demo/avatar/annafali.png',
    imageUrl: '',
    role: 'Admin',
    memberSince: new Date('2023-01-15T10:00:00Z'),
  };

  getSeverityForRole(role: User['role']): string {
    switch (role?.toLowerCase()) {
      case 'admin': return 'danger';
      case 'editor': return 'warning';
      case 'user': return 'info';
      default: return 'secondary';
    }
  }
}
