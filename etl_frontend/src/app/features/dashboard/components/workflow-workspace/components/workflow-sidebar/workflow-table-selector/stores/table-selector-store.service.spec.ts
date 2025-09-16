import { TestBed } from '@angular/core/testing';

import { TableSelectorStoreService } from './table-selector-store.service';

describe('TableSelectorStoreService', () => {
  let service: TableSelectorStoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TableSelectorStoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
