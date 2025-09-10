import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RenameColumnComponent } from './rename-column.component';

describe('RenameColumnComponent', () => {
  let component: RenameColumnComponent;
  let fixture: ComponentFixture<RenameColumnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RenameColumnComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RenameColumnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
