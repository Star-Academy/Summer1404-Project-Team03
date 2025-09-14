import { TestBed } from '@angular/core/testing';
import { UsersService } from './users.service';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { ChangePasswordResponse, updateUserInfoBody, updateUserInfoResponse, UserInfo } from '../../models/user.model';
import { environment } from '../../../../environments/environment';

describe('UsersService', () => {
  let service: UsersService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        UsersService,
        provideHttpClient(),
        provideHttpClientTesting(),
      ]
    });
    service = TestBed.inject(UsersService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getChangePasswordUrl', () => {
    it('should send a GET request to fetch the change password URL', () => {
      const mockResponse: ChangePasswordResponse = { changePasswordUrl: 'https://account.provider.com/change-password' };

      service.getChangePasswordUrl().subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(environment.api.users.password);
      expect(req.request.method).toBe('GET');

      req.flush(mockResponse);
    });
  });

  describe('getUserInformation', () => {
    it('should send a GET request to fetch user info', () => {
      const mockUserInfo: UserInfo = {
        id: 'user-123',
        email: 'test@example.com',
        firstName: 'Test',
        lastName: 'User',
        username: 'testuser',
        roles: [{ id: 'role-abc', name: 'Admin' }]
      };

      service.getUserInformation().subscribe(userInfo => {
        expect(userInfo).toEqual(mockUserInfo);
      });

      const req = httpMock.expectOne(environment.api.auth.me);
      expect(req.request.method).toBe('GET');

      req.flush(mockUserInfo);
    });
  });

  describe('updateUserInformation', () => {
    it('should send a PUT request with the user info to the update endpoint', () => {
      const newUserInfo: updateUserInfoBody = {
        firstName: 'UpdatedFirst',
        lastName: 'UpdatedLast',
        email: 'updated@example.com'
      };

      const mockResponse: updateUserInfoResponse = {
        id: 'user-123',
        username: 'testuser',
        firstName: 'UpdatedFirst',
        lastName: 'UpdatedLast',
        email: 'updated@example.com'
      };

      service.updateUserInformation(newUserInfo).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(environment.api.auth.me);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(newUserInfo);

      req.flush(mockResponse);
    });
  });
});