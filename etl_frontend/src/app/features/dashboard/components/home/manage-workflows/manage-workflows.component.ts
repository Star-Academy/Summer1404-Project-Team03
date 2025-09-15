import { Component, computed, effect, OnInit, signal } from '@angular/core';
import { MessageService } from 'primeng/api';
import { WorkflowsListStore } from '../../../stores/workflows-list/workflows-list-store.service';
import { WorkflowService } from './service/workflow.service';

@Component({
  selector: 'app-manage-workflows',
  standalone: false,
  templateUrl: './manage-workflows.component.html',
  styleUrl: './manage-workflows.component.scss'
})
export class ManageWorkflowsComponent {
  public readonly workflows = computed(() => this.workflowListStore.vm().workflows);
  public readonly isLoading = computed(() => this.workflowListStore.vm().isLoadingWorkflows);
  public readonly isEditWorkflowModalVisible = signal<boolean>(false);
  public readonly selectedWorkflowToEdit = signal<string>('');

  constructor(
    private readonly workflowListStore: WorkflowsListStore,
    private readonly messageService: MessageService,
  ) {}

  onToggleEditWrokflowModalVisibility(): void {
    this.isEditWorkflowModalVisible.update(val => !val);
  }

  public fetchWorkflows(): void {
    this.workflowListStore.loadWorkflows();
  }

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

  public onEditWorkflow(workflowId: string) {
    this.onToggleEditWrokflowModalVisibility();
    this.selectedWorkflowToEdit.set(workflowId);
  }
}

