import { TestBed } from '@angular/core/testing';

import { TableRowStoreService } from './table-row-store.service';

describe('TableRowStoreService', () => {
  let service: TableRowStoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TableRowStoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
