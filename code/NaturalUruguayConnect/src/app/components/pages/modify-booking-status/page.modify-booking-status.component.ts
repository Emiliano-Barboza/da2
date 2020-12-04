import { Component, OnInit } from '@angular/core';
import {BookingService} from '../../../services/booking/booking.service';
import {Booking} from '../../../models/booking';
import {BaseComponent} from '../../../helpers/baseComponent';
import {Observable} from 'rxjs';
import {SideNavOption} from '../../../models/sideNavOption';
import {BookingStatus} from '../../../models/bookingStatus';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {User} from '../../../models/user';
import {ModalService} from '../../../services/modal/modal.service';

@Component({
  selector: 'app-modify-booking-status',
  templateUrl: './page.modify-booking-status.component.html',
  styleUrls: ['./page.modify-booking-status.component.css']
})
export class PageModifyBookingStatusComponent extends BaseComponent  {
  booking: Booking;
  form: FormGroup;
  goBackUrl = '/bookings';
  justifyContentHeader = 'space-between';
  updateStatusLabel = 'Actualizar estado';
  goBackUrlName = 'Volver a revervas';
  fullNameLabel = 'Nombre completo:';
  emailLabel = 'Email:';
  confirmationCodeLabel = 'Código de reserva';
  statusLabel = 'Estado de reserva';
  statusDescriptionLabel = 'Comentarios del estado de la reserva';
  invalidStatusDescriptionLabel = 'Debe llenar los comentarios';
  contactInfo = 'Información de contacto';
  phoneLabel = 'Teléfono de contacto';
  bookingUpdatedSuccessLabel = 'Reserva actualizada.';
  bookinstatusErrorLabel = 'Error al actualizar la reserva.';
  bookingStatusOptions: Observable<BookingStatus[]>;
  selectedBookingStatus = '';
  bookingStatusDescription = '';

  constructor(private bookingService: BookingService, private formBuilder: FormBuilder, private modalService: ModalService) {
    super();
  }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      bookingStatusName: ['', Validators.required],
      bookingStatusDescription: ['', Validators.required]
    });
    this.bookingStatusOptions = this.bookingService.bookingStatuses$;
    this.bookingService.booking$.subscribe(booking => {
      this.booking = booking;
      this.form.setValue({
        bookingStatusName: this.booking.bookingStatus.name,
        bookingStatusDescription:  this.booking.statusDescription
      });
    });
  }

  updateBookingStatus(): void {
    this.form.markAllAsTouched();
    if (this.form.valid) {
      const bookingStatus: BookingStatus = {
        name: this.form.get('bookingStatusName').value,
        description: this.form.get('bookingStatusDescription').value
      };
      this.bookingService.updateBookingStatus(this.booking.id, bookingStatus).subscribe(() => {
        this.modalService.openSuccessSnackBar(this.bookingUpdatedSuccessLabel);
      }, (error) => {
        this.modalService.openErrorSnackBar(this.bookinstatusErrorLabel);
      });
    }
  }
}
