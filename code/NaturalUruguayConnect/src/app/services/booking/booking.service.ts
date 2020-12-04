import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import {Booking} from '../../models/booking';
import {HttpClient, HttpParams} from '@angular/common/http';
import {User} from '../../models/user';
import {PaginatedModel} from '../../models/paginatedModel';
import {PaginatedCountModel} from '../../models/paginatedCountModel';
import {AuthService} from '../auth/auth.service';
import {BookingStatus} from '../../models/bookingStatus';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private baseUrl = '';
  private booking: Booking = {};
  private readonly bookingSource = new BehaviorSubject<Booking>(this.booking);
  readonly booking$ = this.bookingSource.asObservable();
  private bookings: Booking[] = [];
  private readonly bookingsSource = new BehaviorSubject<Booking[]>(this.bookings);
  readonly bookings$ = this.bookingsSource.asObservable();
  private counts: PaginatedCountModel;
  private readonly totalSource = new BehaviorSubject<number>(0);
  readonly total$ = this.totalSource.asObservable();
  private readonly pageSizeSource = new BehaviorSubject<number>(0);
  readonly pageSize$ = this.pageSizeSource.asObservable();
  private bookingStatuses: BookingStatus[] = [
    {
      name: 'Creada'
    },
    {
      name: 'Pendiente Pago'
    },
    {
      name: 'Aceptada'
    },
    {
      name: 'Rechazada'
    },
    {
      name: 'Expirada'
    }
  ];
  private readonly bookingStatusesSource = new BehaviorSubject<BookingStatus[]>(this.bookingStatuses);
  readonly bookingStatuses$ = this.bookingStatusesSource.asObservable();

  constructor(private http: HttpClient, private authService: AuthService) {
    this.baseUrl = environment.apiUrl;
  }

  getBooking(bookingCode: string, options: HttpParams = new HttpParams()): Observable<Booking> {
    const endpoint = this.baseUrl + 'bookings/' + bookingCode + '/status';
    const request = this.http.get<Booking>(endpoint, { params: options });
    request.subscribe(response => {
      this.bookingSource.next(response);
    });
    return request;
  }

  getBookings(params: HttpParams = new HttpParams()): Observable<{data: Booking[]}> {
    const endpoint = this.baseUrl + 'bookings';
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken() };
    const request = this.http.get<PaginatedModel>(endpoint, { headers, params });
    request.subscribe(response => {
      this.counts = response.counts;
      this.bookingsSource.next(response.data);
      this.totalSource.next(this.counts.total);
      this.pageSizeSource.next(this.counts.paging.limit);
    });
    return request;
  }

  updateBookingStatus(bookingId: number, bookingStatus: BookingStatus): Observable<Booking>  {
    const endpoint = this.baseUrl + 'bookings/' + bookingId + '/status';
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken() };
    const subscription = this.http.put<User>(endpoint, bookingStatus,  { headers });
    return subscription;
  }
}
