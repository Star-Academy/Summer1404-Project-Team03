import { TestBed } from '@angular/core/testing';

import { TableRowService } from './table-row.service';

describe('TableRowService', () => {
  let service: TableRowService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TableRowService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
