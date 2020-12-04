import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {FormControl, FormGroup} from '@angular/forms';
import {MatMenuTrigger} from '@angular/material/menu';
import {DateService} from '../../services/date/date.service';

@Component({
  selector: 'app-booking-options',
  templateUrl: './booking-options.component.html',
  styleUrls: ['./booking-options.component.css']
})
export class BookingOptionsComponent implements OnInit {
  description = 'Descripción del alojamiento';
  checkInText = 'Llegada';
  checkOutText = 'Salida';
  datesText = 'Elige las fechas de estadía';
  menuText = 'Agregar Huéspedes';
  range: FormGroup;
  checkInDefault = new Date();
  checkOutDefault = new Date();
  defaultDateOffset = 10;
  placeHolderAdults = 'Adultos';
  amountOfAdults = 0;
  hintAdults = '13 años o más';
  placeHolderUnderAge = 'Niños';
  amountOfUnderAge = 0;
  hintUnderAge = 'De 2 a 12';
  placeHolderBabies = 'Bebés';
  amountOfBabies = 0;
  hintBabies = 'Menos de 2';
  placeHolderVeterans = 'Adultos mayores';
  amountOfVeterans = 0;
  hintVeterans = '65 o más';
  isInvalid = false;
  oneDay = 24 * 60 * 60 * 1000;

  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger;
  @Input() buttonLabel = 'Action';
  @Output() buttonEvent = new EventEmitter<any>();

  constructor(private dateService: DateService) { }

  ngOnInit(): void {
    this.checkInDefault.setDate(this.checkInDefault.getDate());
    this.checkOutDefault.setDate(this.checkOutDefault.getDate() + this.defaultDateOffset);
    this.range = new FormGroup({
      checkIn: new FormControl(this.checkInDefault),
      checkOut: new FormControl(this.checkOutDefault)
    });
  }

  actionHandler(): void {
    this.buttonEvent.emit();
  }

  counterChanged(counterType: string, count: number): void {
    if (counterType === this.placeHolderAdults) {
      this.amountOfAdults = count;
    } else if (counterType === this.placeHolderUnderAge) {
      this.amountOfUnderAge = count;
    } else if (counterType === this.placeHolderBabies) {
      this.amountOfBabies = count;
    } else if (counterType === this.placeHolderVeterans) {
      this.amountOfVeterans = count;
    }
  }

  datePickerFilterDates = (d: Date | null): boolean => {
    const day = (d || new Date());
    const today = new Date();
    return day >= today;
  }

  getCheckIn(): Date {
    return this.range.value.checkIn;
  }

  getCheckOut(): Date {
    return this.range.value.checkOut;
  }

  getCheckInTicks(): number {
    return this.dateService.converToTicks(this.range.value.checkIn);
  }

  getCheckOutTicks(): number {
    return this.dateService.converToTicks(this.range.value.checkOut);
  }

  getRangeDays(): number {
    return Math.round(Math.abs((this.checkInDefault.getTime() - this.checkOutDefault.getTime()) / this.oneDay))
  }

  validate(): boolean {
    const isValid = (this.amountOfAdults > 0 || this.amountOfUnderAge > 0 ||
                     this.amountOfBabies > 0 || this.amountOfVeterans > 0);
    this.isInvalid = !isValid;
    return isValid;
  }
}
