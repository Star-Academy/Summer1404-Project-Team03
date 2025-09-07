import { TestBed } from '@angular/core/testing';

import { FilesManagementStore } from './files-management.service';

describe('FilesManagementStore', () => {
  let service: FilesManagementStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FilesManagementStore);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
