import { Directive, effect, inject, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { UserStoreService } from '../../stores/user-store.service';

@Directive({
  selector: '[hasRole]'
})
export class HasRoleDirective {
  private readonly tpl = inject(TemplateRef<any>);
  private readonly vcr = inject(ViewContainerRef);
  private readonly userStore = inject(UserStoreService);

  private roles: string[] = [];

  @Input({ required: true })
  set hasRole(roles: string[] | string) {
    this.roles = Array.isArray(roles) ? roles : [roles];
    this.updateView();
  }

  constructor() {
    effect(() => {
      this.updateView();
    });
  }

  private updateView() {
    const userRoles = this.userStore.vm().user.roles.map(r => r.name);
    const allowed = this.roles.some(r => userRoles.includes(r));

    this.vcr.clear();
    if (allowed) {
      this.vcr.createEmbeddedView(this.tpl);
    }
  }

}
