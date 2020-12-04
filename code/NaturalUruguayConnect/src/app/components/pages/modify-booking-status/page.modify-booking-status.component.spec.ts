import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageModifyBookingStatusComponent } from './modify-booking-status.component';

describe('ModifyBookingStatusComponent', () => {
  let component: PageModifyBookingStatusComponent;
  let fixture: ComponentFixture<PageModifyBookingStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageModifyBookingStatusComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageModifyBookingStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
