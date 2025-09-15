import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../../../../environments/environment';
import { WorkflowInfo, WorkflowPost, WorkflowPut } from '../models/workflow.model';
import { Observable } from 'rxjs';

@Injectable()
export class WorkflowService {
  private readonly workflowApi = environment.api.workflows;
  constructor(private readonly http: HttpClient) { }

  public getAllWorkFlows(): Observable<{ workflows: WorkflowInfo[] }> {
    return this.http.get<{ workflows: WorkflowInfo[] }>(this.workflowApi.list);
  }

  public createWorkflow(newWorkflow: WorkflowPost): Observable<WorkflowInfo> {
    return this.http.post<WorkflowInfo>(this.workflowApi.list, newWorkflow);
  }

  public updateWorkflowById(workflowId: string, updatedWorkflow: WorkflowPut): Observable<WorkflowInfo> {
    return this.http.put<WorkflowInfo>(this.workflowApi.update(workflowId), updatedWorkflow);
  }

  public getWorkflowById(workflowId: string): Observable<WorkflowInfo> {
    return this.http.get<WorkflowInfo>(this.workflowApi.item(workflowId));
  }

  public deleteWorkflowById(workflowId: string) {
    return this.http.delete(this.workflowApi.delete(workflowId));
  }
}
