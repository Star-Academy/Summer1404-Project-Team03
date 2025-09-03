import { TestBed } from '@angular/core/testing';

import { UploadFileStoreService } from './upload-file-store.service';

describe('UploadFileStoreService', () => {
  let service: UploadFileStoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UploadFileStoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
