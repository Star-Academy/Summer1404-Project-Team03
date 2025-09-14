import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import {finalize, tap } from 'rxjs';
import {RowResponse, RowStore, RowType } from '../models/row.model';
import { TableRowService } from '../services/table-row.service';

const initialTables = {
  rows: [],
  offset: 0,
  isLoading: false,
}
@Injectable()
export class TableRowStoreService extends ComponentStore<RowStore>{
  constructor(private readonly http: TableRowService) {
    super(initialTables);
  }

  private readonly rows = this.selectSignal((state) => state.rows);
  private readonly offset = this.selectSignal((state) => state.offset);
  private readonly isLoading = this.selectSignal((state) => state.isLoading);

  private readonly setRows = this.updater((state, rows: RowType[]) => ({...state, rows}));
  private readonly setOffset = this.updater((state, offset: number) => ({...state, offset}));
  private readonly setLoading = this.updater((state, value: boolean) => ({
    ...state,
    isLoading: value
  }));

  public readonly vm = this.selectSignal(
    this.rows,
    this.isLoading,
    (rows, isLoading) => ({rows, isLoading})
  );

  public loadRows(schemaId: number, limit: number = 7): void {
    this.setLoading(true);

    this.http.getTableRows(schemaId, this.offset(), limit).pipe(
      tap({
        next: (responses: RowResponse) => {
          this.setRows(responses.rows)
          this.setOffset(responses.nextOffset)
        },
      }),
      finalize(() => this.setLoading(false)),
    ).subscribe();
  }
}
