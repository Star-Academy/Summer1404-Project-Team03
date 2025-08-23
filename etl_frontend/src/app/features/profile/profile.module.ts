import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { profileRoutes } from './profile.module.routing';
import { ProfileComponent } from './profile.component';
import { ButtonModule } from 'primeng/button';
import { TabsModule } from 'primeng/tabs';
import { MessageService } from 'primeng/api';
import { Button } from 'primeng/button';
import { HeaderComponent } from '../../shared/components/header/header.component';

@NgModule({
  declarations: [
    ProfileComponent,
  ],
  imports: [
    CommonModule,
    Button,
    RouterModule.forChild(profileRoutes),
    ButtonModule,
    TabsModule,
    RouterModule,
    HeaderComponent
  ],
  providers: [
    MessageService,
  ]
})
export class ProfileModule { }
