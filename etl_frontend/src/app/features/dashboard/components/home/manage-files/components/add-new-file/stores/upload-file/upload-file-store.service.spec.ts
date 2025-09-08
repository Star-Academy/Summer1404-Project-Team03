import { TestBed } from '@angular/core/testing';

import { UploadFileStore } from './upload-file-store.service';

describe('UploadFileStore', () => {
  let service: UploadFileStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UploadFileStore);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
