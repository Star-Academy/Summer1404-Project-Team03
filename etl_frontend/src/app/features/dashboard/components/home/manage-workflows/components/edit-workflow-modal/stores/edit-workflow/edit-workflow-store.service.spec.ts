import { TestBed } from '@angular/core/testing';

import { EditWorkflowStore } from './edit-workflow-store.service';

describe('EditWorkflowStore', () => {
  let service: EditWorkflowStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EditWorkflowStore);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
