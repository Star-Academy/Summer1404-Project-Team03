import {ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';
import {DashboardComponent} from './dashboard.component';
import {UserStoreService} from '../../shared/stores/user-store.service';
import {CheckRoutePipe} from '../../shared/pipe/check-route.pipe';
import {Component, input, NO_ERRORS_SCHEMA} from '@angular/core';

@Component({selector: 'app-workflows-tabs-management', template: ''})
class MockWorkflowsTabsManagementComponent {}

@Component({selector: 'app-workflow-selector', template: ''})
class MockWorkflowSelectorComponent {}

@Component({selector: 'app-profile-popover', template: ''})
class MockProfilePopoverComponent {
  username = input<string>;
  isLoading = input<boolean>;
}

@Component({selector: 'app-header', template: '<ng-content></ng-content>'})
class MockHeaderComponent {}

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;
  let mockUserStore: jasmine.SpyObj<UserStoreService>;

  beforeEach(async () => {
    mockUserStore = jasmine.createSpyObj('UserStoreService', ['loadUser', 'vm']);

    await TestBed.configureTestingModule({
      imports: [
        MockWorkflowsTabsManagementComponent,
        MockWorkflowSelectorComponent,
        MockProfilePopoverComponent,
        MockHeaderComponent,
        CheckRoutePipe,
      ],
      declarations: [
        DashboardComponent,
      ],
      providers: [
        {provide: UserStoreService, useValue: mockUserStore}
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call loadUser on init', fakeAsync(() => {
    mockUserStore.vm.and.returnValue({
      user: {id: '', email: '', firstName: '', lastName: '', roles: [], username: ''},
      isLoading: false,
      isSysAdmin: false
    });

    fixture.detectChanges();
    tick();

    expect(mockUserStore.loadUser).toHaveBeenCalled();
  }));

  it('should expose user and isLoading from user store', () => {
    const mockVm = {
      user: {id: '123', email: 'test@test.com', firstName: 'Test', lastName: 'User', roles: [], username: 'testuser'},
      isLoading: true,
      isSysAdmin: false,
    };

    mockUserStore.vm.and.returnValue(mockVm);
    fixture.detectChanges();

    expect(component.user()).toEqual(mockVm.user);
    expect(component.isLoading()).toBeTrue();
  });

  it('should have menu items defined', () => {
    expect(component.menuItems.length).toBeGreaterThan(0);
    expect(component.menuItems[0].label).toBe('DataWave');
  });
});
