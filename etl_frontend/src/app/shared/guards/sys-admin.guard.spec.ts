import { TestBed } from '@angular/core/testing';
import { CanMatchFn } from '@angular/router';

import { sysAdminGuard } from './sys-admin.guard';

describe('sysAdminGuard', () => {
  const executeGuard: CanMatchFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => sysAdminGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
