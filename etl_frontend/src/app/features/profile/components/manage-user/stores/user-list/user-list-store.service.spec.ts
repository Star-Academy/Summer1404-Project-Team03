import { TestBed } from '@angular/core/testing';

import { UserListStoreService } from './user-list-store.service';

describe('UserListStoreService', () => {
  let service: UserListStoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserListStoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
