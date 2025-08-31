import { TestBed } from '@angular/core/testing';

import { CreateUserStoreService } from './create-user-store.service';

describe('CreateUserStoreService', () => {
  let service: CreateUserStoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreateUserStoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
