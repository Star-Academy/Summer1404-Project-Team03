import { TestBed } from '@angular/core/testing';

import { UserListStore } from './user-list-store.service';

describe('UserListStore', () => {
  let service: UserListStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserListStore);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
