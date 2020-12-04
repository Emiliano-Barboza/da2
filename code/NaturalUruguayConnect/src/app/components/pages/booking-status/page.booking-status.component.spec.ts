import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageBookingStatusComponent } from './booking-status.component';

describe('BookingStatusComponent', () => {
  let component: PageBookingStatusComponent;
  let fixture: ComponentFixture<PageBookingStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageBookingStatusComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageBookingStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
