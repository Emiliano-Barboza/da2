import { Component } from '@angular/core';
import {MatDialogConfig} from '@angular/material/dialog';
import {BookingDetailsComponent} from '../../dialogs/booking-details/booking-details.component';
import {ModalService} from '../../../services/modal/modal.service';
import {BaseComponent} from '../../../helpers/baseComponent';
import {BookingService} from '../../../services/booking/booking.service';
import {FormControl, Validators} from '@angular/forms';

@Component({
  selector: 'app-booking-status',
  templateUrl: './page.booking-status.component.html',
  styleUrls: ['./page.booking-status.component.css']
})
export class PageBookingStatusComponent extends BaseComponent {
  bookingCodeLabel = 'Ingrese código de reserva';
  searchBookingLabel = 'Buscar reserva';
  bookingCode: FormControl;

  constructor(private bookingService: BookingService, private modalService: ModalService) {
    super();
  }

  ngOnInit(): void {
    this.bookingCode = new FormControl('', [Validators.required]);
  }

  searchBooking(): void {
    this.bookingService.getBooking(this.bookingCode.value).subscribe(
      (booking) => {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.data = booking;
        this.modalService.open(BookingDetailsComponent, dialogConfig);
      }
    );
  }

}
