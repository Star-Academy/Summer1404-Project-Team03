import { TestBed } from '@angular/core/testing';

import { TableSchemaStoreService } from './table-schema-store.service';

describe('TableSchemaStoreService', () => {
  let service: TableSchemaStoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TableSchemaStoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
