import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageBookingsComponent } from './bookings.component';

describe('BookingsComponent', () => {
  let component: PageBookingsComponent;
  let fixture: ComponentFixture<PageBookingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageBookingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageBookingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
