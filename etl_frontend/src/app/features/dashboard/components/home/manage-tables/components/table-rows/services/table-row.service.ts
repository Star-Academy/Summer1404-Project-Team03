import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../../../../../../environments/environment';
import { Observable } from 'rxjs';
import {RowResponse, RowType } from '../models/row.model';

@Injectable()
export class TableRowService {
  private readonly rowApi = environment.api.rows;

  constructor(private readonly http: HttpClient) { }

  public getTableRows(schemaId: number, offset: number, limit: number): Observable<RowResponse> {
    return this.http.get<RowResponse>(this.rowApi.list(schemaId, offset, limit));
  }
}
