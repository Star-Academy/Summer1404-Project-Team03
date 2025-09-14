import { TestBed } from '@angular/core/testing';
import { HttpClient } from '@angular/common/http';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { errorHandlingInterceptor } from './error-handling.interceptor';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { UserStoreService } from '../../stores/user-store.service';

describe('errorHandlingInterceptor', () => {
  let http: HttpClient;
  let httpMock: HttpTestingController;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockMessageService: jasmine.SpyObj<MessageService>;
  let mockUserStore: jasmine.SpyObj<UserStoreService>;

  beforeEach(() => {
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockMessageService = jasmine.createSpyObj('MessageService', ['add']);
    mockUserStore = jasmine.createSpyObj('UserStoreService', ['clearUser']);

    TestBed.configureTestingModule({
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: MessageService, useValue: mockMessageService },
        { provide: UserStoreService, useValue: mockUserStore },
        provideHttpClient(withInterceptors([errorHandlingInterceptor])),
        provideHttpClientTesting(),
      ],
    });

    http = TestBed.inject(HttpClient);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should handle 401 error by redirecting, clearing user, and showing warning', () => {
    http.get('/api/test').subscribe({
      next: () => fail('expected error'),
      error: () => {}
    });

    const req = httpMock.expectOne('/api/test');
    req.flush({ message: 'Unauthorized' }, { status: 401, statusText: 'Unauthorized' });

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/landing']);
    expect(mockUserStore.clearUser).toHaveBeenCalled();
    expect(mockMessageService.add).toHaveBeenCalledWith(jasmine.objectContaining({
      severity: 'warn',
      summary: 'Session Expired',
      detail: 'Your session has expired. Please log in again.',
      sticky: true,
    }));
  });

  it('should handle other errors by showing error message', () => {
    http.get('/api/test').subscribe({
      next: () => fail('expected error'),
      error: () => {}
    });

    const req = httpMock.expectOne('/api/test');
    req.flush({ message: 'Something went wrong' }, { status: 500, statusText: 'Internal Server Error' });

    expect(mockMessageService.add).toHaveBeenCalledWith(jasmine.objectContaining({
      severity: 'error',
      summary: 'Error 500',
      detail: 'Something went wrong',
      sticky: true,
    }));

    expect(mockRouter.navigate).not.toHaveBeenCalled();
    expect(mockUserStore.clearUser).not.toHaveBeenCalled();
  });
});
