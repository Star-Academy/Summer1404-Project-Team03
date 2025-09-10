import { Injectable } from '@angular/core';
import { Schema, SchemaEditorState } from '../../models/schema.model';
import { ComponentStore } from '@ngrx/component-store';
import { delay, exhaustMap, finalize, map, of, tap } from 'rxjs';
import { FilesManagementService } from '../../../../services/files-management/files-management.service';

const initialState: SchemaEditorState = {
  error: null,
  isFetching: false,
  isSaving: false,
  schema: null,
  dbTypes: [],
  isSaveSuccess: null
}

@Injectable()
export class SchemaEditorStore extends ComponentStore<SchemaEditorState> {
  constructor(private readonly filesManagementService: FilesManagementService) {
    super(initialState);
  }

  public readonly vm = this.selectSignal((s) => s);

  public readonly isSaveSuccess$ = this.select((s) => s.isSaveSuccess);

  public readonly setFetching = this.updater((state, value: boolean) => ({
    ...state,
    isFetching: value,
  }));
  
  public readonly setSaving = this.updater((state, value: boolean) => ({
    ...state,
    isSaving: value,
  }));

  public readonly setSchema = this.updater<Schema>((state, value: Schema) => ({
    ...state,
    schema: value,
  }));

  public readonly getDbTypes = this.effect<void>(($trigger) => {
    return $trigger.pipe(
      tap(() => {
        this.setFetching(true);
        this.patchState({ dbTypes: ['string', 'number', 'float', 'boolean'] });
      })
    );
  });

  public readonly getFileSchema = this.effect<{ fileId: string }>(
    ($trigger) => {
      return $trigger.pipe(
        tap(() => this.setFetching(true)),
        exhaustMap((data: { fileId: string }) =>
          this.filesManagementService.fetchFileSchema(data.fileId).pipe(
            delay(3000),
            tap((res) => {
              this.setSchema(res);
            }),
            finalize(() => this.setFetching(false))
          )
        )
      );
    }
  );

  public readonly updateSchema = this.effect<void>(($trigger) => {
    return $trigger.pipe(
      tap(() => this.setSaving(true)),
      exhaustMap(() => {
        const schema = this.get().schema;
        if (!schema) return of();

        return this.filesManagementService
          .updateSchema(schema)
          .pipe(
            delay(3000),
            tap({
              next: () => {
                this.patchState({ isSaveSuccess: true, error: null });
              },
              error: (err) => {
                this.patchState({ isSaveSuccess: false, error: err });
              }
            }),
            finalize(() => this.setSaving(false))
          );
      })
    );
  });
}
