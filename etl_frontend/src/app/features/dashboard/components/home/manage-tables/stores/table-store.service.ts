import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { TableStore, TableType } from '../models/tables.model';
import { TableService } from '../services/table.service';
import {finalize, tap } from 'rxjs';

const initialTables = {
  tables: [],
  isLoading: false,
}
@Injectable()
export class TableStoreService extends ComponentStore<TableStore>{
  constructor(private readonly http: TableService) {
    super(initialTables);
  }

  private readonly tables = this.selectSignal((state) => state.tables);
  private readonly isLoading = this.selectSignal((state) => state.isLoading);

  private readonly setTables = this.updater((state, tables: TableType[]) => ({...state, tables}));
  private readonly setLoading = this.updater((state, value: boolean) => ({
    ...state,
    isLoading: value
  }));

  public readonly vm = this.selectSignal(
    this.tables,
    this.isLoading,
    (user, isLoading) => ({user, isLoading})
  );

  public loadTables(): void {
    this.setLoading(true);

    this.http.getTable().pipe(
      tap({
        next: (tables: TableType[]) => this.setTables(tables),
      }),
      finalize(() => this.setLoading(false)),
    ).subscribe();
  }
}
