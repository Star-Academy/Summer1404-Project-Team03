import { TestBed, fakeAsync, tick } from '@angular/core/testing';
import { UserStoreService } from './user-store.service';
import { UsersService } from '../services/user/users.service';
import { UserInfo } from '../models/user.model';
import { of, throwError } from 'rxjs';

const initialUser = {
  user: { email: '', firstName: '', id: '', lastName: '', roles: [], username: '' },
  isLoading: false,
  isSysAdmin: false,
};

fdescribe('UserStoreService', () => {
  let store: UserStoreService;
  let usersServiceSpy: jasmine.SpyObj<UsersService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('UsersService', ['getUserInformation']);

    TestBed.configureTestingModule({
      providers: [
        UserStoreService,
        { provide: UsersService, useValue: spy }
      ]
    });

    store = TestBed.inject(UserStoreService);
    usersServiceSpy = TestBed.inject(UsersService) as jasmine.SpyObj<UsersService>;
  });

  it('should be created with initial state', () => {
    expect(store).toBeTruthy();
    expect(store.vm()).toEqual(initialUser);
  });

  it('should set loading, fetch a user, and update the state on success', fakeAsync(() => {
    const mockUser: UserInfo = {
      id: 'user-123', email: 'test@example.com', firstName: 'Test',
      lastName: 'User', username: 'testuser', roles: [{ id: 'role-1', name: 'user' }]
    };
    usersServiceSpy.getUserInformation.and.returnValue(of(mockUser));

    expect(store.vm().isLoading).toBe(false);

    store.loadUser();

    expect(store.vm().isLoading).toBe(true);

    tick();

    const finalState = store.vm();
    expect(finalState.isLoading).toBe(false);
    expect(finalState.user).toEqual(mockUser);
    expect(finalState.isSysAdmin).toBe(false);
  }));

  it('should set isSysAdmin to true if user has the sys_admin role', fakeAsync(() => {
    const mockAdmin: UserInfo = {
      id: 'admin-456', email: 'admin@example.com', firstName: 'Sys', lastName: 'Admin',
      username: 'sysadmin', roles: [{ id: 'role-1', name: 'user' }, { id: 'role-2', name: 'sys_admin' }]
    };
    usersServiceSpy.getUserInformation.and.returnValue(of(mockAdmin));

    store.loadUser();
    tick();

    const finalState = store.vm();
    expect(finalState.user).toEqual(mockAdmin);
    expect(finalState.isSysAdmin).toBe(true);
  }));

  it('should handle API errors and set loading to false', fakeAsync(() => {
    usersServiceSpy.getUserInformation.and.returnValue(throwError(() => new Error('API Error')));

    store.loadUser();

    expect(store.vm().isLoading).toBe(true);

    tick();

    const finalState = store.vm();
    expect(finalState.isLoading).toBe(false);
    expect(finalState.user).toEqual(initialUser.user);
    expect(finalState.isSysAdmin).toBe(false);
  }));

  it('should reset the state to its initial value', fakeAsync(() => {
    const mockUser: UserInfo = { id: 'user-123', email: 'test@example.com', firstName: 'Test', lastName: 'User', username: 'testuser', roles: [] };
    usersServiceSpy.getUserInformation.and.returnValue(of(mockUser));
    store.loadUser();
    tick();

    expect(store.vm()).not.toEqual(initialUser);

    store.clearUser();

    expect(store.vm()).toEqual(initialUser);
  }));
});