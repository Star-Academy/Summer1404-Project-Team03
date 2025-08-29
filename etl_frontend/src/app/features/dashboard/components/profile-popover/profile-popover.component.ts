import {Component, input, ViewChild} from '@angular/core';
import {Button} from "primeng/button";
import {Popover} from 'primeng/popover';
import {SignOutDirective} from '../../../../shared/directives/sign-out/sign-out.directive';
import {RouterModule} from "../../../../../../node_modules/@angular/router";
import { Skeleton } from 'primeng/skeleton';

@Component({
  selector: 'app-profile-popover',
  imports: [
    Button,
    Popover,
    Skeleton,
    SignOutDirective,
    RouterModule
  ],
  templateUrl: './profile-popover.component.html',
  styleUrl: './profile-popover.component.scss'
})
export class ProfilePopoverComponent {
  public readonly username = input.required<string>();
  public readonly isLoading = input.required<boolean>();

  @ViewChild('op') op!: Popover;

  public readonly options = [
    {label: 'Account', link: '/profile', icon: 'pi pi-user'},
    {label: 'Sign out', link: '', icon: 'pi pi-sign-out'},
  ];

  public toggle(event: Event) {
    event.stopPropagation();
    this.op.toggle(event);
  }
}
