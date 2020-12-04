import {Component, Inject, Input, OnInit} from '@angular/core';
import { Booking } from '../../../models/booking';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';

@Component({
  selector: 'app-booking-details',
  templateUrl: './booking-details.component.html',
  styleUrls: ['./booking-details.component.css']
})
export class BookingDetailsComponent implements OnInit {
  booking: Booking;
  fullNameLabel = 'Nombre completo:';
  emailLabel = 'Email:';
  priceLabel = 'Precio:';
  confirmationCodeLabel = 'Código de reserva';
  statusLabel = 'Estado de reserva';
  contactInfo = 'Información de contacto';
  phoneLabel = 'Teléfono de contacto';
  titleLabel = 'Datos de la reserva';
  closeLabel = 'Cerrar';

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<BookingDetailsComponent>){
    this.booking = data.context;
  }

  ngOnInit(): void {}

  close(): void {
    this.dialogRef.close();
  }
}
