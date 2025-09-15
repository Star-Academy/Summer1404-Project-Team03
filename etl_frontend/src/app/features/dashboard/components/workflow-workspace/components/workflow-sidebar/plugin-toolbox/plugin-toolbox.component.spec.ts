import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PluginToolboxComponent } from './plugin-toolbox.component';

describe('PluginToolboxComponent', () => {
  let component: PluginToolboxComponent;
  let fixture: ComponentFixture<PluginToolboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PluginToolboxComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PluginToolboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
