import { TestBed } from '@angular/core/testing';

import { EditUserStoreService } from './edit-user-store.service';

describe('EditUserStoreService', () => {
  let service: EditUserStoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EditUserStoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
