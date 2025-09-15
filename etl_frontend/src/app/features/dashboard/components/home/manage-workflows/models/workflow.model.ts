export type WorkflowInfo = {
  id: string;
  name: string;
  description: string;
  createdAt: Date;
  updatedAt: Date;
  status: 'Draft' | 'Running' | 'Completed' | 'Failed';
}

export type WorkflowPost = {
  name: string;
  description?: string;
}

export type WorkflowPut = {
  name: string;
  description: string;
  status: string;
}

export type WorkflowsListState = {
  workflows: WorkflowInfo[];
  openedWorkflowsId: string[];
  editingWorkflowsId: string[];
  selectedWorkflowId: string | null;
  isLoadingWorkflows: boolean;
  error: string | null;
  isCreatingWorkflow: boolean;
  loadingWorkflowId: string | null;
}

export type WorkflowEditState = {
  workflow: WorkflowInfo | undefined;
  isLoading: boolean;
  error: string | null;
}