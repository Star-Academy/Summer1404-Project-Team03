import { TestBed } from '@angular/core/testing';

import { WorkflowsListStore } from './workflows-list-store.service';

describe('WorkflowsListStore', () => {
  let service: WorkflowsListStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WorkflowsListStore);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
