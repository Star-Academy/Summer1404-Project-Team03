import { Component } from '@angular/core';
import { WorkflowsListStore } from '../../stores/workflows-list/workflows-list-store.service';

@Component({
  selector: 'app-workflows-tabs-management',
  standalone: false,
  templateUrl: './workflows-tabs-management.component.html',
  styleUrl: './workflows-tabs-management.component.scss'
})
export class WorkflowsTabsManagementComponent {
  public readonly vm;
  public readonly openWorkflows$;
  constructor(public readonly workflowListStore: WorkflowsListStore) {
    this.vm = this.workflowListStore.vm;
    this.openWorkflows$ = this.workflowListStore.getOpendWorkflows()
  }
}
