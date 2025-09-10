import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../../../../environments/environment';
import { Observable } from 'rxjs';
import { TableType } from '../models/tables.model';

@Injectable()
export class TableService {
  private readonly tableApi = environment.api.tables;
  private readonly fileApi = environment.api.files;

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

  public createTable(fileId: number) {
    return this.http.post(this.fileApi.registerAndLoad(fileId), { mode: "Append", dropOnFailure: false });
  }
}