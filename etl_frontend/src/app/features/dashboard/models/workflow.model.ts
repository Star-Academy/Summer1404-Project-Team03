export interface WorkflowInfo {
    id: string;
    name: string;
    description?: string;
    createdAt?: Date;
    updatedAt?: Date;
    status?: 'draft' | 'running' | 'completed' | 'failed';
}

export interface WorkflowsListState {
    workflows: WorkflowInfo[];
    openedWorkflowsId: string[];
    selectedWorkflowId: string | null;
    isLoadingWorkflows: boolean;
    error: string | null;
    isCreatingWorkflow: boolean;
    loadingWorkflowId: string | null;
}
