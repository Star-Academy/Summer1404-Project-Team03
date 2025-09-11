import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../../../../../../environments/environment';
import { Observable } from 'rxjs';
import { ColumnType } from '../models/column.model';

@Injectable()
export class TableColumnService {
  private readonly columnApi = environment.api.columns;
  constructor(private readonly http: HttpClient) { }

  public getTableColumns(schemaId: number): Observable<ColumnType[]> {
    return this.http.get<ColumnType[]>(this.columnApi.list(schemaId));
  }

  public deleteTableColumn(schemaId: number, columnIds: number[]) {
    return this.http.delete(this.columnApi.list(schemaId), {
      body: { columnIds }
    });
  }

  public renameTableColumn(schemaId: number, columnId: number, newName: string) {
    return this.http.post(this.columnApi.rename(schemaId, columnId), newName);
  }
}
