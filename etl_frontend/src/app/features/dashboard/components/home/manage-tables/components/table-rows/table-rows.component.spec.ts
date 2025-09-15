import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableRowsComponent } from './table-rows.component';

describe('TableRowsComponent', () => {
  let component: TableRowsComponent;
  let fixture: ComponentFixture<TableRowsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableRowsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TableRowsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
