import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import {finalize, tap } from 'rxjs';
import { ColumnStore, ColumnType } from '../models/column.model';
import { TableColumnService } from '../services/table-column.service';

const initialTables = {
  columns: [],
  isLoading: false,
}
@Injectable()
export class TableColumnStoreService extends ComponentStore<ColumnStore>{
  constructor(private readonly http: TableColumnService) {
    super(initialTables);
  }

  private readonly columns = this.selectSignal((state) => state.columns);
  private readonly isLoading = this.selectSignal((state) => state.isLoading);

  private readonly setColumns = this.updater((state, columns: ColumnType[]) => ({...state, columns}));
  private readonly setLoading = this.updater((state, value: boolean) => ({
    ...state,
    isLoading: value
  }));

  public readonly vm = this.selectSignal(
    this.columns,
    this.isLoading,
    (columns, isLoading) => ({columns, isLoading})
  );

  public loadColumn(schemaId: number): void {
    this.setLoading(true);

    this.http.getTableColumns(schemaId).pipe(
      tap({
        next: (column:{ items: ColumnType[] }) => {
          this.setColumns(column.items)
        },
      }),
      finalize(() => this.setLoading(false)),
    ).subscribe();
  }
}
