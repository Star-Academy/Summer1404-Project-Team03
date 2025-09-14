import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';

import { AuthService } from './auth.service';
import { provideHttpClient } from '@angular/common/http';
import { SendTokenCodeBody, SendTokenCodeResponse, SignInResponse } from '../../models/auth.model';
import { environment } from '../../../../environments/environment';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        AuthService,
      ]
    });
    httpMock = TestBed.inject(HttpTestingController);
    service = TestBed.inject(AuthService);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should send a POST request to the sign-in endpoint and return the response', () => {
    const expectedRes: SignInResponse = { redirectUrl: 'http://auth.datawave.ir/?user=someone' }
    const expectedBody = { redirectUrl: environment.redirectUrl };

    service.getSignInUrl().subscribe(res => expect(res).toEqual(expectedRes));

    const req = httpMock.expectOne(environment.api.auth.signIn);

    expect(req.request.method).toBe("POST");
    expect(req.request.body).toEqual(expectedBody);

    req.flush(expectedRes);
  });

  it('SHOULD send a POST request to the sign-out endpoint and RETURN the response', () => {
    const expectedBody = {}
    const expectedRes = "you are signed out"

    service.signOut().subscribe(res => expect(res).toEqual(expectedRes))

    const req = httpMock.expectOne(environment.api.auth.signOut);

    expect(req.request.method).toBe("POST");
    expect(req.request.body).toEqual(expectedBody);

    req.flush(expectedRes)
  });

  it('SHOULD send a POST request to the token endpoint and ', () => {
    const expectedCode: string = "public code...";
    const expectedBody: SendTokenCodeBody = { code: "public code...", redirectUrl: environment.redirectUrl };
    const expectedRes: SendTokenCodeResponse = { redirectUrl: environment.redirectUrl };

    service.sendTokenCode(expectedCode).subscribe(res => expect(res).toEqual(expectedRes));

    const req = httpMock.expectOne(environment.api.auth.token);

    expect(req.request.method).toBe("POST");
    expect(req.request.body).toEqual(expectedBody);

    req.flush(expectedRes);
  })
});
