import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import {RouterService} from '../services/router/router.service';
import {BookingService} from '../services/booking/booking.service';
import {Booking} from '../models/booking';

@Injectable({ providedIn: 'root' })
export class BookingsByConfirmationCodeResolver implements Resolve<Booking> {
  constructor(private bookingService: BookingService, private routerService: RouterService) {}

  resolve(route: ActivatedRouteSnapshot) {
    const confirmationCodeParam = this.routerService.confirmationCodeParam;
    const confirmationCode = route.paramMap.get(confirmationCodeParam);
    const user = this.bookingService.getBooking(confirmationCode);
    return user;
  }
}
