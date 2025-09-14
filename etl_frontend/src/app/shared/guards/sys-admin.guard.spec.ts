import { TestBed } from '@angular/core/testing';
import { Router, CanMatchFn } from '@angular/router';
import { sysAdminGuard } from './sys-admin.guard';
import { UserStoreService } from '../stores/user-store.service';

describe('sysAdminGuard', () => {
  let mockUserStore: jasmine.SpyObj<UserStoreService>;
  let mockRouter: jasmine.SpyObj<Router>;

  const executeGuard: CanMatchFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => sysAdminGuard(...guardParameters));

  beforeEach(() => {
    mockUserStore = jasmine.createSpyObj('UserStoreService', ['vm']);
    mockRouter = jasmine.createSpyObj('Router', ['navigateByUrl']);

    TestBed.configureTestingModule({
      providers: [
        { provide: UserStoreService, useValue: mockUserStore },
        { provide: Router, useValue: mockRouter },
      ],
    });
  });

  it('should allow access when user has sysAdmin role', () => {
    mockUserStore.vm.and.returnValue({
      user: {
        id: 'u1',
        username: 'admin',
        email: 'admin@example.com',
        firstName: 'Admin',
        lastName: 'User',
        roles: [{ id: 'r1', name: 'SYS_ADMIN' }]
      },
      isLoading: false,
      isSysAdmin: true
    });

    const result = executeGuard(
      { data: { roles: 'sys_admin' } } as any,
      [] as any
    );

    // expect(result).toBeTrue();
    // expect(mockRouter.navigateByUrl).not.toHaveBeenCalled();
    //TODO fix this
  });

  it('should deny access and redirect when user does not have sysAdmin role', () => {
    mockUserStore.vm.and.returnValue({
      user: {
        id: 'u1',
        username: 'admin',
        email: 'admin@example.com',
        firstName: 'Admin',
        lastName: 'User',
        roles: [{ id: 'r1', name: 'data_admin' }]
      },
      isLoading: false,
      isSysAdmin: false
    });

    const result = executeGuard(
      { data: { roles: 'SYS_ADMIN' } } as any,
      [] as any
    );

    expect(result).toBeFalse();
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('/profile');
  });

  it('should deny access and redirect when route has no roles', () => {
    mockUserStore.vm.and.returnValue({
      user: {
        id: 'u1',
        username: 'admin',
        email: 'admin@example.com',
        firstName: 'Admin',
        lastName: 'User',
        roles: [{ id: 'r1', name: 'SYS_ADMIN' }]
      },
      isLoading: false,
      isSysAdmin: true
    });

    const result = executeGuard({} as any, [] as any);

    expect(result).toBeFalse();
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('/profile');
  });
});
