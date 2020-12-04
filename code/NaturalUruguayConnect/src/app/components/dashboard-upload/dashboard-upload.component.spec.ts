import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardUploadComponent } from './dashboard-upload.component';

describe('DashboardUploadComponent', () => {
  let component: DashboardUploadComponent;
  let fixture: ComponentFixture<DashboardUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DashboardUploadComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
