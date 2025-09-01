import { Component } from '@angular/core';
import { WorkflowsListStore } from '../../stores/workflows-list/workflows-list-store.service';
import { of } from 'rxjs';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-workflows-tabs-management',
  standalone: false,
  templateUrl: './workflows-tabs-management.component.html',
  styleUrl: './workflows-tabs-management.component.scss'
})
export class WorkflowsTabsManagementComponent {
  public readonly vm;

  constructor(public readonly workflowListStore: WorkflowsListStore) {
    this.vm = this.workflowListStore.vm;
  }

  onCreateNewWorkflow(): void {
    this.workflowListStore.createNewWorkflow(of({ workflowName: "New Workflow" }));
  }

  openWorkflow(workflowId: string): void {
    this.workflowListStore.openWorkflow(of({ workflowId }));
  }

  onDrop(event: CdkDragDrop<any[]>) {
    moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    this.workflowListStore.reorderOpenWorkflows(event.container.data);
  }
}
