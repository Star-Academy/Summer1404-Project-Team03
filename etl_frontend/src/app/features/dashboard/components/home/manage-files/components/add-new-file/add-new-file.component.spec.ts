import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddNewFileComponent } from './add-new-file.component';

describe('AddNewFileComponent', () => {
  let component: AddNewFileComponent;
  let fixture: ComponentFixture<AddNewFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddNewFileComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddNewFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
