import { Component, ViewChild } from '@angular/core';
import { Button } from "primeng/button";
import { Popover } from 'primeng/popover';
import { SignOutDirective } from '../../../../shared/directives/sign-out/sign-out.directive';
import { RouterModule } from "../../../../../../node_modules/@angular/router";

@Component({
  selector: 'app-profile-popover',
  imports: [
    Button,
    Popover,
    SignOutDirective,
    RouterModule
  ],
  templateUrl: './profile-popover.component.html',
  styleUrl: './profile-popover.component.scss'
})
export class ProfilePopoverComponent {
  @ViewChild('op') op!: Popover;

  public readonly options = [
    { label: 'Account', link: '/dashboard/profile', icon: 'pi pi-user' },
    { label: 'Sign out', link: '', icon: 'pi pi-sign-out' },
  ];

  public toggle(event: Event) {
    this.op.toggle(event);
  }

  public selectOption(link: string) {
    this.op.hide();
  }
}
