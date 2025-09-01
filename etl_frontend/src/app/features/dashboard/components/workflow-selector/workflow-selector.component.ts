import { Component } from '@angular/core';
import { WorkflowsListStore } from '../../stores/workflows-list/workflows-list-store.service';
import { WorkflowInfo } from '../../models/workflow.model';
import { of } from 'rxjs';
import { ListboxChangeEvent } from 'primeng/listbox';

@Component({
  selector: 'app-workflow-selector',
  standalone: false,
  templateUrl: './workflow-selector.component.html',
  styleUrl: './workflow-selector.component.scss'
})
export class WorkflowSelectorComponent {
  public readonly vm;

  constructor(private readonly workflowsListStore: WorkflowsListStore) {
    this.vm = this.workflowsListStore.vm;
  }

change(event: ListboxChangeEvent): void {
  this.workflowsListStore.openWorkflow(of({ workflowId: event.value.id }));
}
}
