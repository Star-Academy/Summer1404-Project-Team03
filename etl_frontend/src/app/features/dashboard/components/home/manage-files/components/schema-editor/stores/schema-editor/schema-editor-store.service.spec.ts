import { TestBed } from '@angular/core/testing';

import { SchemaEditorStore } from './schema-editor-store.service';

describe('SchemaEditorStore', () => {
  let service: SchemaEditorStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SchemaEditorStore);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
