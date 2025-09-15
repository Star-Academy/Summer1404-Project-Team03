import { Component, computed, effect, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { WorkflowsListStore } from '../../../stores/workflows-list/workflows-list-store.service';
import { WorkflowService } from './service/workflow.service';

interface FlowsColumn {
  header: string;
  selectedType: string;
}
@Component({
  selector: 'app-manage-workflows',
  standalone: false,
  templateUrl: './manage-workflows.component.html',
  styleUrl: './manage-workflows.component.scss'
})
export class ManageWorkflowsComponent implements OnInit{
  public readonly workflows = computed(() => this.workflowListStore.vm().workflows);
  public readonly isLoading = computed(() => this.workflowListStore.vm().isLoadingWorkflows);

  constructor(
    private readonly workflowListStore: WorkflowsListStore,
    private readonly workflowService: WorkflowService,
    private readonly messageService: MessageService,
  ) {}

  public onDeleteWorkflow(workflowId: string, workflowStatus: string) {
    if (workflowStatus === 'Running') {
      this.messageService.add({
        severity: 'error',
        summary: 'Workflow is Running. First you need to close it.',
      });
      return;
    }

    this.workflowListStore.deleteWorkflow(workflowId);
  }

  public onEditWorkflow(workflowId: string) {}

  ngOnInit() {
    this.workflowListStore.loadWorkflows();
  }
}

