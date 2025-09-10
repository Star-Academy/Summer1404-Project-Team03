import { Injectable } from '@angular/core';
import { Schema, SchemaEditorState } from '../../models/schema.model';
import { ComponentStore } from '@ngrx/component-store';
import { exhaustMap, finalize, map, of, tap } from 'rxjs';
import { FilesManagementService } from '../../../../services/files-management/files-management.service';

const initialState: SchemaEditorState = {
  error: null,
  isLoading: false,
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

  public readonly setLoading = this.updater((state, value: boolean) => ({
    ...state,
    isLoading: value,
  }));

  public readonly setSchema = this.updater<Schema>((state, value: Schema) => ({
    ...state,
    schema: value,
  }));

  public readonly getDbTypes = this.effect<void>(($trigger) => {
    return $trigger.pipe(
      tap(() => {
        this.setLoading(true);
        this.patchState({ dbTypes: ['string', 'number', 'float', 'boolean'] });
      })
    );
  });

  public readonly getFileSchema = this.effect<{ fileId: string }>(
    ($trigger) => {
      return $trigger.pipe(
        tap(() => this.setLoading(true)),
        exhaustMap((data: { fileId: string }) =>
          this.filesManagementService.fetchFileSchema(data.fileId).pipe(
            tap((res) => {
              this.setSchema(res);
            }),
            finalize(() => this.setLoading(false))
          )
        )
      );
    }
  );

  public readonly updateSchema = this.effect<void>(($trigger) => {
    return $trigger.pipe(
      tap(() => this.setLoading(true)),
      exhaustMap(() => {
        const schema = this.get().schema;
        if (!schema) return of();

        return this.filesManagementService
          .updateSchema(schema)
          .pipe(
            tap({
              next: () => {
                this.patchState({ isSaveSuccess: true, error: null });
              },
              error: (err) => {
                this.patchState({ isSaveSuccess: false, error: err });
              }
            }),
            finalize(() => this.setLoading(false))
          );
      })
    );
  });
}
