import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { WorkflowEditState, WorkflowPut } from '../../../../models/workflow.model';
import { catchError, exhaustMap, of, Subject, tap } from 'rxjs';
import { WorkflowService } from '../../../../service/workflow.service';

const initialState: WorkflowEditState = {
  error: null,
  isLoading: false,
  workflow: undefined
};

@Injectable()
export class EditWorkflowStore extends ComponentStore<WorkflowEditState> {
  private readonly editResultSubject = new Subject<'success' | 'error'>();
  readonly editResult$ = this.editResultSubject.asObservable();

  constructor(private readonly workflowService: WorkflowService) {
    super(initialState);
  }

  public readonly vm = this.selectSignal((s) => s);

  readonly getWorkflow = this.effect<string>((workflowId$) =>
    workflowId$.pipe(
      tap(() => this.patchState({ isLoading: true, error: null })),
      exhaustMap((workflowId) =>
        this.workflowService.getWorkflowById(workflowId).pipe(
          tap((workflow) => this.patchState({ workflow, isLoading: false })),
          catchError((error) => {
            this.patchState({ error, isLoading: false });
            return of(error);
          })
        )
      )
    )
  );

  readonly editWorkflow = this.effect<{ editedWorkflow: WorkflowPut; workflowId: string }>((trigger$) =>
    trigger$.pipe(
      tap(() => this.patchState({ isLoading: true, error: null })),
      exhaustMap(({ workflowId, editedWorkflow }) => {
        console.log(workflowId, editedWorkflow);
        return this.workflowService.updateWorkflowById(workflowId, editedWorkflow).pipe(
          tap({
            next: (workflow) => {
              this.patchState({ workflow, isLoading: false });
              this.editResultSubject.next('success');
            },
            error: (error) => {
              this.patchState({ error, isLoading: false });
              this.editResultSubject.next('error');
            },
          })
        )
      })
    )
  );
}
