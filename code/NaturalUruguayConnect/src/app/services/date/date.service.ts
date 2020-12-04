import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateService {
  private readonly dateOffset = 621355968000000000;
  private readonly dayOffset = 10000;

  constructor() { }

  getDate(rawDate: string): Date {
    let date = null;
    if (rawDate) {
      date = new Date(rawDate);
    } else {
      date = new Date();
    }
    return date;
  }

  getFormattedDate(date: Date): string {
    let formattedDate = '';
    if (date) {
      formattedDate = date.getDate() + '-' + (date.getMonth() + 1) + '-' + date.getFullYear();
    }
    return formattedDate;
  }

  convertTicksToDateString(stringTick: string): string {
    const ticks = Number(stringTick);
    const time = (ticks - this.dateOffset) / this.dayOffset;
    const date = new Date(time);
    let formattedDate = '';
    if (date) {
      formattedDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();
    }
    return formattedDate;
  }

  converToTicks(date: Date): number {
    const dateToConvert = date.getTime();
    const ticks = (dateToConvert * this.dayOffset) + this.dateOffset;
    return ticks;
  }
}
