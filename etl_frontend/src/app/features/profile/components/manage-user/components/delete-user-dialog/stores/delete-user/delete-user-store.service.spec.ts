import { TestBed } from '@angular/core/testing';

import { DeleteUserStore } from './delete-user-store.service';

describe('DeleteUserStore', () => {
  let service: DeleteUserStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeleteUserStore);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
