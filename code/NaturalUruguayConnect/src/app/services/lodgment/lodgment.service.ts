import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import {Lodgment} from '../../models/lodgments';
import {Booking} from '../../models/booking';
import {HttpClient, HttpParams} from '@angular/common/http';
import {BookingConfirmationModel} from '../../models/bookingConfirmationModel';
import {PaginatedModel} from '../../models/paginatedModel';
import {AuthService} from '../auth/auth.service';
import {PaginatedCountModel} from '../../models/paginatedCountModel';
import {Review} from '../../models/review';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LodgmentService {

  private baseUrl = '';
  private lodgment: Lodgment;
  private readonly lodgmentSource = new BehaviorSubject<Lodgment>(null);
  readonly lodgment$ = this.lodgmentSource.asObservable();
  private lodgments: Lodgment[] = [];
  private readonly lodgmentsSource = new BehaviorSubject<Lodgment[]>(this.lodgments);
  readonly lodgments$ = this.lodgmentsSource.asObservable();
  private reviews: Review[] = [];
  private readonly reviewsSource = new BehaviorSubject<Review[]>(this.reviews);
  readonly reviews$ = this.reviewsSource.asObservable();
  private counts: PaginatedCountModel;
  private readonly totalSource = new BehaviorSubject<number>(0);
  readonly total$ = this.totalSource.asObservable();
  private readonly pageSizeSource = new BehaviorSubject<number>(0);
  readonly pageSize$ = this.pageSizeSource.asObservable();

  constructor(private http: HttpClient, private authService: AuthService) {
    this.baseUrl = environment.apiUrl;
  }

  getLodgment(id: number, options: HttpParams = new HttpParams()): Observable<Lodgment> {
    const endpoint = this.baseUrl + 'lodgments/' + id;
    const request = this.http.get<Lodgment>(endpoint, { params: options });
    request.subscribe(response => {
      this.lodgment = response;
      this.lodgmentSource.next(this.lodgment);
    });
    return request;
  }

  createReview(id: number, data: Review): Observable<Review> {
    const endpoint = this.baseUrl + 'lodgments/' + id + '/reviews';
    const request = this.http.post<Review>(endpoint, data);
    return request;
  }

  getLodgmentReviews(id: number, params: HttpParams = new HttpParams()): Observable<{data: Lodgment[]}> {
    const endpoint = this.baseUrl + 'lodgments/' + id + '/reviews';
    const headers = { Authorization: 'Bearer ' + this.authService.getToken() };
    const request = this.http.get<PaginatedModel>(endpoint, { headers, params });
    request.subscribe(response => {
      this.counts = response.counts;
      this.reviewsSource.next(response.data);
      this.totalSource.next(this.counts.total);
      this.pageSizeSource.next(this.counts.paging.limit);
    });
    return request;
  }

  getLodgments(params: HttpParams = new HttpParams()): Observable<{data: Lodgment[]}> {
    const endpoint = this.baseUrl + 'lodgments';
    const headers = { Authorization: 'Bearer ' + this.authService.getToken() };
    const request = this.http.get<PaginatedModel>(endpoint, { headers, params });
    request.subscribe(response => {
      this.counts = response.counts;
      this.lodgmentsSource.next(response.data);
      this.totalSource.next(this.counts.total);
      this.pageSizeSource.next(this.counts.paging.limit);
    });
    return request;
  }

  createBooking(id: number, data: BookingConfirmationModel): Observable<Booking> {
    const endpoint = this.baseUrl + 'lodgments/' + id + '/bookings/';
    const request = this.http.post<Booking>(endpoint, data);
    return request;
  }

  deleteLodgment(lodgment: Lodgment): Observable<Lodgment>  {
    const endpoint = this.baseUrl + 'lodgments/' + lodgment.id;
    const headers = { Authorization: 'Bearer ' + this.authService.getToken() };
    const subscription = this.http.delete<Lodgment>(endpoint,  { headers });
    return subscription;
  }

  activateLodgment(lodgment: Lodgment): Observable<Lodgment>  {
    const endpoint = this.baseUrl + 'lodgments/' + lodgment.id + '/activate';
    const headers = { Authorization: 'Bearer ' + this.authService.getToken() };
    const subscription = this.http.post(endpoint, null,  { headers });
    return subscription;
  }

  deactivateLodgment(lodgment: Lodgment): Observable<Lodgment>  {
    const endpoint = this.baseUrl + 'lodgments/' + lodgment.id + '/deactivate';
    const headers = { Authorization: 'Bearer ' + this.authService.getToken() };
    const subscription = this.http.delete(endpoint,  { headers });
    return subscription;
  }
}
