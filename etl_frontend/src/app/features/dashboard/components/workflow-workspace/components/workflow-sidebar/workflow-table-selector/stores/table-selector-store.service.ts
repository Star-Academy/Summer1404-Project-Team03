import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { TableType } from '../../../../../home/manage-tables/models/tables.model';
import { TableService } from '../../../../../home/manage-tables/services/table.service';
import { finalize, tap } from 'rxjs';

type tableSelectorState = {
  isLoading: boolean;
  tables: TableType[];
  selectedTableId: string | null;
  error: null | string;
}

const initialState: tableSelectorState = {
  isLoading: false,
  tables: [],
  error: null,
  selectedTableId: null
}

@Injectable()
export class TableSelectorStore extends ComponentStore<tableSelectorState> {

  constructor(private readonly tableService: TableService) {
    super(initialState);
    this.loadTables();
  }

  public readonly vm = this.selectSignal(s => s);

  public loadTables(): void {
    this.patchState({ isLoading: true });

    this.tableService.getTable().pipe(
      tap({
        next: (tables: TableType[]) => this.patchState({ tables: tables }),
      }),
      finalize(() => this.patchState({ isLoading: true }))
    ).subscribe();
  }
}
