import { TestBed } from '@angular/core/testing';

import { WorkflowsListManagementService } from './workflows-list-management.service';

describe('WorkflowsListManagementService', () => {
  let service: WorkflowsListManagementService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WorkflowsListManagementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
