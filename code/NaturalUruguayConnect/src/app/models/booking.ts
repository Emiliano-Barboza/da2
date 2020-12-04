import { Lodgment } from './lodgments';
import { BookingStatus } from './bookingStatus';
export interface Booking {
  id?: number;
  name?: string;
  lastName?: string;
  email?: string;
  checkIn?: bigint;
  checkout?: bigint;
  confirmationCode?: string;
  price?: number;
  statusDescription?: string;
  lodgment?: Lodgment;
  bookingStatus?: BookingStatus;
}
