import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../../../../environments/environment';
import { Observable } from 'rxjs';
import { TableType } from '../models/tables.model';

@Injectable()
export class TableService {
  private readonly tableApi = environment.api.tables;

  constructor(private readonly http: HttpClient) { }

  public getTable(): Observable<TableType[]> {
    return this.http.get<TableType[]>(this.tableApi.list);
  }

  public deleteTable(schemaId: string) {
    return this.http.delete(this.tableApi.delete(schemaId));
  }

  public renameTable(schemaId: number, newTableName: string) {
    return this.http.post(this.tableApi.rename(schemaId), newTableName);
  }
}
