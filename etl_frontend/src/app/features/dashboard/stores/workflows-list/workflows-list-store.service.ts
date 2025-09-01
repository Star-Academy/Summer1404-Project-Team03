import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { WorkflowInfo, WorkflowsListState } from '../../models/workflow.model';
import { catchError, exhaustMap, of, switchMap, tap, throwError } from 'rxjs';

const initialState: WorkflowsListState = {
  workflows: [{
    id: 'wf_001',
    name: 'Customer Segmentation',
    description: 'Cluster customers based on purchase history.',
    createdAt: new Date('2025-07-15T10:30:00Z'),
    updatedAt: new Date('2025-08-20T14:10:00Z'),
    status: 'draft'
  },
  {
    id: 'wf_002',
    name: 'Sales Data Cleaning',
    description: 'Remove duplicates and fix missing values in sales dataset.',
    createdAt: new Date('2025-08-01T09:00:00Z'),
    updatedAt: new Date('2025-08-25T16:45:00Z'),
    status: 'completed'
  },
  {
    id: 'wf_003',
    name: 'Churn Prediction',
    description: 'Predict customer churn using logistic regression.',
    createdAt: new Date('2025-08-10T11:15:00Z'),
    updatedAt: new Date('2025-08-29T18:20:00Z'),
    status: 'running'
  },
  {
    id: 'wf_004',
    name: 'Marketing Campaign Analysis',
    description: 'Analyze effectiveness of email campaigns.',
    createdAt: new Date('2025-07-28T13:00:00Z'),
    updatedAt: new Date('2025-08-27T09:50:00Z'),
    status: 'failed'
  },
  {
    id: 'wf_005',
    name: 'Inventory Forecasting',
    description: 'Time-series forecasting of inventory demand.',
    createdAt: new Date('2025-08-05T15:45:00Z'),
    updatedAt: new Date('2025-08-30T12:00:00Z'),
    status: 'draft'
  }],
  error: null,
  isLoadingWorkflows: false,
  openedWorkflowsId: ['wf_005', 'wf_004', 'wf_001'],
  selectedWorkflowId: 'wf_005',
  loadingWorkflowId: null,
  isCreatingWorkflow: false,
}

@Injectable()
export class WorkflowsListStore extends ComponentStore<WorkflowsListState> {

  constructor() {
    super(initialState);
    //TODO handle selected workflowId at begining
  }

  public readonly vm = this.selectSignal(s => {
    return {
      ...s,
      openWorkflows: s.workflows.filter((wf) =>
        s.openedWorkflowsId.includes(wf.id)
      )
    }
  }
  );

  public createNewWorkflow = this.effect<{ workflowName: string }>(workflow$ => {
    return workflow$.pipe(
      tap(() => this.patchState({ isCreatingWorkflow: true })),
      exhaustMap(({ workflowName }) => {
        // TODO: replace with actual service call
        return of({
          id: 'wf_' + Math.floor(Math.random() * 10000),
          name: workflowName,
          description: 'New workflow created by user.',
          createdAt: new Date(),
          updatedAt: new Date(),
          status: 'running'
        } as WorkflowInfo).pipe(
          tap((newWorkflow) => {
            this.patchState({
              workflows: [...this.vm().workflows, newWorkflow],
              openedWorkflowsId: [...this.vm().openedWorkflowsId, newWorkflow.id],
              selectedWorkflowId: newWorkflow.id,
              isCreatingWorkflow: false
            });
          }),
          catchError((error) => {
            this.patchState({ isCreatingWorkflow: false, error });
            return of(error);
          })
        );
      })
    );
  });

  public openWorkflow = this.effect<{ workflowId: string }>(workflow$ => {
    return workflow$.pipe(
      tap((workflow) => this.patchState({ loadingWorkflowId: workflow.workflowId })),
      switchMap((wf) => {
        const index = this.vm().workflows.findIndex((workflow) => workflow.id == wf.workflowId)
        return index !== -1 ? of(this.vm().workflows[index]) : throwError('can not find workflow');
      }),
      tap((workflow) => this.patchState({ loadingWorkflowId: null, selectedWorkflowId: workflow.id }))
    )
  })

}