import { TestBed } from '@angular/core/testing';
import { TableColumnStoreService } from './table-column-store.service';

describe('TableColumnStoreService', () => {
  let service: TableColumnStoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TableColumnStoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
