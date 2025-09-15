import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { catchError, exhaustMap, of, switchMap, tap, throwError } from 'rxjs';
import {WorkflowInfo, WorkflowsListState } from '../../components/home/manage-workflows/models/workflow.model';
import { WorkflowService } from '../../components/home/manage-workflows/service/workflow.service';

const initialState: WorkflowsListState = {
  workflows: [],
  error: null,
  isLoadingWorkflows: false,
  openedWorkflowsId: [],
  selectedWorkflowId: null,
  loadingWorkflowId: null,
  isCreatingWorkflow: false,
}

@Injectable()
export class WorkflowsListStore extends ComponentStore<WorkflowsListState> {

  constructor(private readonly http: WorkflowService) {
    super(initialState);
    //TODO handle selected workflowId at begining
  }

  public readonly vm = this.selectSignal(s => {
    const workflowsArray = Array.isArray(s.workflows) ? s.workflows : [];
    const workflowMap = new Map(workflowsArray.map(wf => [wf.id, wf]));

    const openWorkflows = s.openedWorkflowsId
      .map(id => workflowMap.get(id))
      .filter((wf): wf is WorkflowInfo => wf !== undefined);

    return {
      ...s,
      openWorkflows
    }
  });

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
          status: 'Running'
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
      tap(({ workflowId }) =>
        this.patchState({ loadingWorkflowId: workflowId })
      ),
      switchMap(({ workflowId }) => {
        const workflow = this.get().workflows.find(wf => wf.id === workflowId);

        if (!workflow) {
          return throwError(() => new Error('Cannot find workflow')).pipe(
            catchError(err => {
              this.patchState({ loadingWorkflowId: null });
              return throwError(() => err);
            })
          );
        }

        return of(workflow).pipe(
          tap(wf =>
            this.patchState((state) => {
              if (state.openedWorkflowsId.includes(wf.id)) {
                return {
                  loadingWorkflowId: null,
                  selectedWorkflowId: wf.id,
                };
              }

              return {
                loadingWorkflowId: null,
                selectedWorkflowId: wf.id,
                openedWorkflowsId: [...state.openedWorkflowsId, wf.id],
              };
            })
          )
        );
      })
    );
  });


  public readonly reorderOpenWorkflows = this.updater((state, reorderedWorkflows: WorkflowInfo[]) => {
    const newOrderedIds = reorderedWorkflows.map(wf => wf.id);

    return {
      ...state,
      openedWorkflowsId: newOrderedIds
    };
  });

  public readonly closeWorkflow = this.updater(
    (state, workflowIdToClose: string) => {
      const idx = state.openedWorkflowsId.indexOf(workflowIdToClose);
      const newOpenedIds = state.openedWorkflowsId.filter(id => id !== workflowIdToClose);

      if (state.selectedWorkflowId !== workflowIdToClose) {
        return {
          ...state,
          openedWorkflowsId: newOpenedIds,
        };
      }

      let newSelectedId: string | null = null;
      if (newOpenedIds.length > 0) {
        if (idx < newOpenedIds.length) {
          newSelectedId = newOpenedIds[idx];
        } else {
          newSelectedId = newOpenedIds[idx - 1];
        }
      }

      return {
        ...state,
        openedWorkflowsId: newOpenedIds,
        selectedWorkflowId: newSelectedId,
      };
    }
  );

  public loadWorkflows = this.effect<void>(trigger$ =>
    trigger$.pipe(
      tap(() => this.patchState({ isLoadingWorkflows: true, error: null })),
      switchMap(() =>
        this.http.getAllWorkFlows().pipe(
          tap((res: {workflows: WorkflowInfo[] }) => {
            this.patchState({
              workflows: res.workflows,
              isLoadingWorkflows: false,
            });
          }),
          catchError((error) => {
            this.patchState({ isLoadingWorkflows: false, error });
            return of(error);
          })
        )
      )
    )
  );
}
