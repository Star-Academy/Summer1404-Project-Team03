import { TestBed } from '@angular/core/testing';

import { DeleteUserStoreService } from './delete-user-store.service';

describe('DeleteUserStoreService', () => {
  let service: DeleteUserStoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeleteUserStoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
