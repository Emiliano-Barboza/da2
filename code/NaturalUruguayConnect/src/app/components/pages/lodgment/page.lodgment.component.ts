import { Component, ViewChild } from '@angular/core';
import {LodgmentService} from '../../../services/lodgment/lodgment.service';
import {Lodgment} from '../../../models/lodgments';
import {BookingConfirmationModel} from '../../../models/bookingConfirmationModel';
import {FormControl, Validators} from '@angular/forms';
import {LodgmentOptionsModel} from '../../../models/lodgmentOptionsModel';
import {BookingOptionsComponent} from '../../booking-options/booking-options.component';
import {BaseComponent} from '../../../helpers/baseComponent';
import {takeUntil, take} from 'rxjs/operators';
import {ModalService} from '../../../services/modal/modal.service';
import {BookingDetailsComponent} from '../../dialogs/booking-details/booking-details.component';
import {MatDialogConfig} from '@angular/material/dialog';
import {Review} from '../../../models/review';
import {HttpParams} from '@angular/common/http';

@Component({
  selector: 'app-lodgment',
  templateUrl: './page.lodgment.component.html',
  styleUrls: ['./page.lodgment.component.css']
})
export class PageLodgmentComponent extends BaseComponent{
  bookingText = 'Reservar';
  contactInfo = 'Información de contacto';
  description = 'Descripción del alojamiento';
  email: FormControl;
  emailLabel = 'Ingresa un email:';
  emailPlaceholder = 'Por ej. juan@gmail.com';
  goBackUrl = '/spots/';
  goBackUrlName = '';
  lastName: FormControl;
  lastNameLabel = 'Apellido:';
  lastNamePlaceholder = 'Por ej. Pérez';
  location = 'Ubicación del alojamiento';
  phone = 'Teléfono de contacto';
  lodgment: Lodgment;
  lodgmentName = '';
  lodgmentStars: number;
  lodgmentSummary = '';
  currency = 'USD ';
  name: FormControl;
  nameLabel = 'Nombre:';
  namePlaceholder = 'Por ej. Juan';
  priceDescription = 'Precio por noche';
  priceByNight = '';
  totalLabel = 'Total';
  imageHeight = '200px';
  total: number;
  pageSize: number;
  reviews: Review[];

  @ViewChild(BookingOptionsComponent) bookingOptions;

  constructor(private lodgmentService: LodgmentService, private modalService: ModalService) {
    super();
  }

  ngOnInit(): void {
    this.name = new FormControl('', [Validators.required]);
    this.lastName = new FormControl('', [Validators.required]);
    this.email = new FormControl('', [Validators.required, Validators.email]);
    this.lodgmentService.lodgment$.pipe(takeUntil(this.destroyed)).subscribe(lodgment => {
      this.lodgment = lodgment;
      this.lodgmentName = lodgment.name;
      this.lodgmentStars = lodgment.amountOfStars;
      this.goBackUrl += lodgment.spot.id + '/lodgments';
      this.goBackUrlName = lodgment.spot.name + ', ' + lodgment.spot.region.name;
      this.priceByNight = this.currency + lodgment.price + '/ ' + this.priceDescription;
    });
    this.lodgmentService.reviews$.pipe(takeUntil(this.destroyed)).subscribe(reviews => {
      this.reviews = reviews;
    });
    this.lodgmentService.total$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.total = x;
    });
    this.lodgmentService.pageSize$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.pageSize = x;
    });
  }

  private getErrorMessage(formControl: FormControl): string {
    let error = '';
    if (formControl.hasError('required')) {
      error = 'Debes ingresar un valor.';
    } else {
      error = formControl.hasError('email') ? 'Email inválido.' : '';
    }
    return error;
  }

  private validateBooking(): boolean {
    let isValid = this.name.valid;
    if (!isValid) {
      this.name.markAsTouched();
    }
    isValid = this.lastName.valid && isValid;
    if (!isValid) {
      this.lastName.markAsTouched();
    }
    isValid = this.email.valid && isValid;
    if (!isValid) {
      this.email.markAsTouched();
    }
    isValid = this.bookingOptions.validate() && isValid;

    return isValid;
  }

  private clearForm(): void {
    this.name.reset();
    this.lastName.reset();
    this.email.reset();
  }

  booking(): void {
    const isValid = this.validateBooking();
    if (isValid) {
      const lodgmentOptions: LodgmentOptionsModel = {
        checkIn: this.bookingOptions.getCheckInTicks(),
        checkOut: this.bookingOptions.getCheckOutTicks(),
        amountOfAdults: this.bookingOptions.amountOfAdults,
        amountOfUnderAge: this.bookingOptions.amountOfUnderAge,
        amountOfBabies: this.bookingOptions.amountOfBabies,
        amountOfVeterans: this.bookingOptions.amountOfVeterans
      };
      const booking: BookingConfirmationModel = {
        name: this.name.value,
        lastName: this.lastName.value,
        email: this.email.value,
        lodgmentOptions
      };
      this.lodgmentService.createBooking(this.lodgment.id, booking).pipe(takeUntil(this.destroyed)).subscribe(
        (bookingResponse) => {
          const dialogConfig = new MatDialogConfig();
          dialogConfig.data = bookingResponse;
          this.modalService.open(BookingDetailsComponent, dialogConfig).subscribe(result => {
            this.clearForm();
          });
        }
      );
    }
  }

  createReview(review): void {
    if (this.lodgment) {
      this.lodgmentService.createReview(this.lodgment.id, review).pipe(take(1)).subscribe(
        (reviewResponse) => {
          this.modalService.openSuccessSnackBar('Reseña creada');
          this.searchReviews();
        },
        (dataError) => {
          console.log(dataError);
          const message = dataError.status !== 500 ? dataError.error.detail : 'No se pudo crear la reseña';
          this.modalService.openErrorSnackBar(message);
        }
      );
    }
  }

  searchReviews(params: HttpParams = new HttpParams()): void {
    if (this.lodgment) {
      this.lodgmentService.getLodgmentReviews(this.lodgment.id, params);
    }
  }
}
