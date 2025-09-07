import { TestBed } from '@angular/core/testing';

import { TablesManagementService } from './tables-management.service';

describe('TablesManagementService', () => {
  let service: TablesManagementService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TablesManagementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
