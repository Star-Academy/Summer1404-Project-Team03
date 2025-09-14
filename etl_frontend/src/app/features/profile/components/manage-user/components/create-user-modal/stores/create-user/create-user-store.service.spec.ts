import { TestBed } from '@angular/core/testing';

import { CreateUserStore } from './create-user-store.service';

describe('CreateUserStore', () => {
  let service: CreateUserStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreateUserStore);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
