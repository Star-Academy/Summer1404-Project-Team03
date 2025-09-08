import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadedFileItemComponent } from './uploaded-file-item.component';

describe('UploadedFileItemComponent', () => {
  let component: UploadedFileItemComponent;
  let fixture: ComponentFixture<UploadedFileItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UploadedFileItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UploadedFileItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
