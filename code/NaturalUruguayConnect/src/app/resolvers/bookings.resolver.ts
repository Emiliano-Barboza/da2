import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import {User} from '../models/user';
import {BookingService} from '../services/booking/booking.service';

@Injectable({ providedIn: 'root' })
export class BookingsResolver implements Resolve<{data: User[]}> {
  constructor(private bookingService: BookingService) {}

  resolve() {
    const bookings = this.bookingService.getBookings();
    return bookings;
  }
}
