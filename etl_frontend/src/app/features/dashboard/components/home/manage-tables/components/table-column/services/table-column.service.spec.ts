import { TestBed } from '@angular/core/testing';
import { TableColumnService } from './table-column.service';


describe('TableColumnService', () => {
  let service: TableColumnService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TableColumnService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
