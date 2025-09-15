import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../../../../environments/environment';
import { WorkflowPost, WorkflowPut } from '../models/workflow.model';

@Injectable()
export class WorkflowService {
  private readonly workflowApi = environment.api.workflows;
  constructor(private readonly http: HttpClient) { }

  public getAllWorkFlows() {
    return this.http.get(this.workflowApi.list);
  }

  public createWorkflow(newWorkflow: WorkflowPost) {
    return this.http.post(this.workflowApi.list, newWorkflow);
  }

  public updateWorkflowById(workflowId: string, updatedWorkflow: WorkflowPut) {
    return this.http.put(this.workflowApi.update(workflowId), updatedWorkflow);
  }

  public getWorkflowById(workflowId: string) {
    return this.http.get(this.workflowApi.item(workflowId));
  }

  public deleteWorkflowById(workflowId: string) {
    return this.http.delete(this.workflowApi.delete(workflowId));
  }
}
