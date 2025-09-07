import { Injectable } from '@angular/core';
import { SchemaEditorState } from '../../models/schema.model';
import { ComponentStore } from '@ngrx/component-store';
import { exhaustMap, finalize, tap } from 'rxjs';
import { FilesManagementService } from '../../../../services/files-management/files-management.service';

const initialState: SchemaEditorState = {
  error: null,
  isLoading: false,
  schema: null
}

@Injectable()
export class SchemaEditorStore extends ComponentStore<SchemaEditorState> {

  constructor(private readonly filesManagementService: FilesManagementService) {
    super(initialState);
  }

  public readonly vm = this.selectSignal(s => s);

  public readonly setLoading = this.updater((state, value: boolean) => ({
    ...state,
    isLoading: value
  })
  )

  public readonly getDbTypes = this.effect<void>($trigger => {
    return $trigger.pipe(
      tap(() => this.setLoading(true)),
    )
  })

  public readonly getFileSchema = this.effect<{ fileId: string }>($trigger => {
    return $trigger.pipe(
      tap(() => this.setLoading(true)),
      exhaustMap((data: { fileId: string }) => this.filesManagementService.fetchFileSchema(data.fileId).pipe(
        finalize(() => this.setLoading(false))
      ))
    )
  })
}
