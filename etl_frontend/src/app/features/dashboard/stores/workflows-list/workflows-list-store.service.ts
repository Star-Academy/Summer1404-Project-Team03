import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { catchError, exhaustMap, of, switchMap, tap, throwError } from 'rxjs';
import { WorkflowInfo, WorkflowPost, WorkflowsListState } from '../../components/home/manage-workflows/models/workflow.model';
import { WorkflowService } from '../../components/home/manage-workflows/service/workflow.service';
import { MessageService } from 'primeng/api';

const initialState: WorkflowsListState = {
  workflows: [],
  error: null,
  isLoadingWorkflows: false,
  openedWorkflowsId: [],
  selectedWorkflowId: null,
  loadingWorkflowId: null,
  isCreatingWorkflow: false,
  editingWorkflowsId: []
}

@Injectable()
export class WorkflowsListStore extends ComponentStore<WorkflowsListState> {

  constructor(
    private readonly workflowService: WorkflowService,
    private readonly messageService: MessageService,
  ) {
    super(initialState);
    this.loadWorkflows();
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

  private readonly addEditingWorkflow = this.updater<string>((state, value) =>
    ({ ...state, editingWorkflowsId: [...state.editingWorkflowsId.filter(id => id !== value)] })
  )

  private readonly removeEditingWorkflow = this.updater<string>((state, value) =>
    ({ ...state, editingWorkflowsId: [...state.editingWorkflowsId, value] })
  )

  public createNewWorkflow = this.effect<{ workflowName: string }>(workflow$ => {
    return workflow$.pipe(
      tap(() => this.patchState({ isCreatingWorkflow: true })),
      exhaustMap(({ workflowName }) => {
        const newWorkflow: WorkflowPost = { name: workflowName };
        return this.workflowService.createWorkflow(newWorkflow).pipe(
          tap((createdWorkflow) => {
            createdWorkflow = { ...createdWorkflow, status: "Draft" }
            this.patchState({
              workflows: [...this.get().workflows, createdWorkflow],
              openedWorkflowsId: [...this.get().openedWorkflowsId, createdWorkflow.id],
              selectedWorkflowId: createdWorkflow.id,
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

  public readonly deleteWorkflow = this.effect<string>(workflowId$ =>
    workflowId$.pipe(
      tap(() => this.patchState({ isLoadingWorkflows: true, error: null })),
      exhaustMap((workflowId) =>
        this.workflowService.deleteWorkflowById(workflowId).pipe(
          tap(() => {
            this.patchState((state) => {
              const updatedWorkflows = state.workflows.filter(wf => wf.id !== workflowId);
              const newOpenedIds = state.openedWorkflowsId.filter(id => id !== workflowId);

              let newSelectedId = state.selectedWorkflowId;
              if (state.selectedWorkflowId === workflowId) {
                const idx = state.openedWorkflowsId.indexOf(workflowId);
                if (newOpenedIds.length > 0) {
                  newSelectedId = idx < newOpenedIds.length
                    ? newOpenedIds[idx]
                    : newOpenedIds[idx - 1];
                } else {
                  newSelectedId = null;
                }
              }

              return {
                ...state,
                workflows: updatedWorkflows,
                openedWorkflowsId: newOpenedIds,
                selectedWorkflowId: newSelectedId,
                isLoadingWorkflows: false
              };
            });

            this.messageService.add({
              severity: 'success',
              summary: 'Workflow deleted successfully.',
            });
          }),
          catchError((error) => {
            this.patchState({ isLoadingWorkflows: false, error });
            this.messageService.add({
              severity: 'error',
              summary: 'Workflow delete failed',
            });
            return of(error);
          })
        )
      )
    )
  );
  public loadWorkflows = this.effect<void>(trigger$ =>
    trigger$.pipe(
      tap(() => this.patchState({ isLoadingWorkflows: true, error: null })),
      switchMap(() =>
        this.workflowService.getAllWorkFlows().pipe(
          tap((res: { workflows: WorkflowInfo[] }) => {
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

  public readonly editWorkflow = this.effect<WorkflowInfo>((trigger$) => {
    return trigger$.pipe(
      tap((workflow) => this.addEditingWorkflow(workflow.id)),
      // exhaustMap((editedWorkflow) => this.workflowService.updateWorkflowById(workflow))
    )
  })
}
