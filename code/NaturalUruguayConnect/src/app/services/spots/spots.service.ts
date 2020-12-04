import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Lodgment } from '../../models/lodgments';
import { Spot } from '../../models/spot';
import { PaginatedModel } from '../../models/paginatedModel';
import { PaginatedCountModel } from '../../models/paginatedCountModel';
import {Booking} from '../../models/booking';
import {AuthService} from '../auth/auth.service';
import {ReportModel} from '../../models/reportModel';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SpotsService {

  private baseUrl = '';
  private spot: Spot = {};
  private readonly spotSource = new BehaviorSubject<Spot>(null);
  readonly spot$ = this.spotSource.asObservable();
  private spots: Spot[] = [];
  private readonly spotsSource = new BehaviorSubject<Lodgment[]>(this.spots);
  readonly spots$ = this.spotsSource.asObservable();
  private lodgments: Lodgment[] = [];
  private readonly lodgmentsSource = new BehaviorSubject<Lodgment[]>(this.lodgments);
  readonly lodgments$ = this.lodgmentsSource.asObservable();
  private counts: PaginatedCountModel;
  private readonly totalSource = new BehaviorSubject<number>(0);
  readonly total$ = this.totalSource.asObservable();
  private readonly pageSizeSource = new BehaviorSubject<number>(0);
  readonly pageSize$ = this.pageSizeSource.asObservable();

  constructor(private http: HttpClient, private authService: AuthService) {
    this.baseUrl = environment.apiUrl;
  }

  createLodgment(id: number, lodgment: Lodgment): Observable<Booking> {
    const endpoint = this.baseUrl + 'spots/' + id + '/lodgments/';
    const headers = { Authorization: 'Bearer ' + this.authService.getToken() };
    const request = this.http.post<Lodgment>(endpoint, lodgment, {headers});
    return request;
  }

  getLodgments(id: number, options: HttpParams = new HttpParams()): Observable<PaginatedModel> {
    const endpoint = this.baseUrl + 'spots/' + id + '/lodgments';
    const request = this.http.get<PaginatedModel>(endpoint, { params: options });
    request.subscribe(response => {
      this.lodgments = response.data;
      this.counts = response.counts;
      this.lodgmentsSource.next(this.lodgments);
      this.totalSource.next(this.counts.total);
      this.pageSizeSource.next(this.counts.paging.limit);
    });
    return request;
  }

  getSpot(id: number): Observable<Spot> {
    const endpoint = this.baseUrl + 'spots/' + id;
    const request = this.http.get<Spot>(endpoint);
    request.subscribe(response => {
      this.spot = response;
      this.spotSource.next(response);
    });
    return request;
  }

  getSpots(params: HttpParams = new HttpParams()): void {
    const endpoint = this.baseUrl + 'spots';
    const headers = { Authorization: 'Bearer ' + this.authService.getToken() };
    const request = this.http.get<PaginatedModel>(endpoint, { headers, params });
    request.subscribe(response => {
      this.counts = response.counts;
      this.spotsSource.next(response.data);
      this.totalSource.next(this.counts.total);
      this.pageSizeSource.next(this.counts.paging.limit);
    });
  }

  getSpotReport(id: number, params: HttpParams = new HttpParams()): Observable<ReportModel> {
    const endpoint = this.baseUrl + 'spots/' + id + '/reports/';
    const headers = { Authorization: 'Bearer ' + this.authService.getToken() };
    const request = this.http.get<ReportModel>(endpoint, {headers, params});
    return request;
  }
}
