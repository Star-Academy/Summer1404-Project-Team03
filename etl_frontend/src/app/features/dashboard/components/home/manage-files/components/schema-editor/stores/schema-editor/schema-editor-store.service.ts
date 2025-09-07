import { Injectable } from '@angular/core';
import { SchemaEditorState } from '../../models/schema.model';
import { ComponentStore } from '@ngrx/component-store';
import { exhaustMap, tap } from 'rxjs';

const initialState: SchemaEditorState = {
  error: null,
  isLoading: false,
  schema: null
}

@Injectable()
export class SchemaEditorStore extends ComponentStore<SchemaEditorState> {

  constructor() {
    super(initialState);
  }

  public readonly setLoading = this.updater((state, value: boolean) => ({
    ...state,
    isLoading: value
  })
  )

  public readonly getDbTypes = this.effect<void>($trigger => {
    return $trigger.pipe(
      tap(() => this.setLoading(true)),
      // exhaustMap(() => )
    )
  })
}
