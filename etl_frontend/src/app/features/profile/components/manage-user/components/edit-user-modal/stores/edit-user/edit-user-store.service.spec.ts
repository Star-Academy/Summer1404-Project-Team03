import { TestBed } from '@angular/core/testing';

import { EditUserStore } from './edit-user-store.service';

describe('EditUserStore', () => {
  let service: EditUserStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EditUserStore);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
