import { Component } from '@angular/core';
import { WorkflowsListStore } from '../../stores/workflows-list/workflows-list-store.service';
import { of } from 'rxjs';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import {
  trigger,
  transition,
  style,
  animate,
} from '@angular/animations';

@Component({
  selector: 'app-workflows-tabs-management',
  standalone: false,
  templateUrl: './workflows-tabs-management.component.html',
  styleUrl: './workflows-tabs-management.component.scss',
  animations: [
    trigger('tabAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateX(-1rem) scale(0.7)' }),
        animate('300ms ease-out', style({ opacity: 1, transform: 'translateX(0) scale(1)' })),
      ]),
      transition(':leave', [
        animate('300ms ease-in', style({ opacity: 0, transform: 'translateX(-1rem) scale(0.7)' })),
      ]),
    ]),
  ],
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

  onCloseWorkflow(workflowId: string): void {
    this.workflowListStore.closeWorkflow(workflowId);
  }
}
