import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Region} from '../../models/region';
import {Spot} from '../../models/spot';
import {PaginatedModel} from '../../models/paginatedModel';
import {PaginatedCountModel} from '../../models/paginatedCountModel';
import { environment } from '../../../environments/environment';
import {AuthService} from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class RegionsService {

  private baseUrl = '';
  private readonly regionSource = new BehaviorSubject<Region>(null);
  readonly region$ = this.regionSource.asObservable();
  private regions: Region[] = [];
  private readonly regionsSource = new BehaviorSubject<Region[]>([]);
  readonly regions$ = this.regionsSource.asObservable();
  private spots: Spot[] = [];
  private readonly spotsSource = new BehaviorSubject<Spot[]>([]);
  readonly spots$ = this.spotsSource.asObservable();
  private counts: PaginatedCountModel;
  private readonly totalSource = new BehaviorSubject<number>(0);
  readonly total$ = this.totalSource.asObservable();
  private readonly pageSizeSource = new BehaviorSubject<number>(0);
  readonly pageSize$ = this.pageSizeSource.asObservable();

  constructor(private http: HttpClient, private authService: AuthService) {
    this.baseUrl = environment.apiUrl;
  }

  filterRegionsByName(name: string): void {
    const filterValue = name.toLowerCase();
    let filtered = [];

    if (filterValue) {
      filtered = this.regions.filter(option => option.name.toLowerCase().indexOf(filterValue) >= 0);
    }
    else {
      filtered = this.regions.concat();
    }
    this.regionsSource.next(filtered);
  }

  filterSpotsByName(name: string): Spot[] {
    const filterValue = name.toLowerCase();
    let filtered = [];

    if (filterValue) {
      filtered = this.spots.filter(option => option.name.toLowerCase().indexOf(filterValue) >= 0);
    }
    else {
      filtered = this.spots.concat();
    }
    return filtered;
  }

  createSpot(id: number, spot: Spot): Observable<Spot> {
    const endpoint = this.baseUrl + 'regions/' + id + '/spots/';
    const headers = { Authorization: 'Bearer ' + this.authService.getToken() };
    const request = this.http.post<Spot>(endpoint, spot, {headers});
    return request;
  }

  getRegions(params: HttpParams = new HttpParams()): Observable<{data: Region[]}> {
    const endpoint = this.baseUrl + 'regions';
    const request = this.http.get<PaginatedModel>(endpoint, {params});
    request.subscribe(response => {
      this.regions = response.data;
      this.regionsSource.next(response.data);
    });
    return request;
  }

  getRegion(id: number): void {
    const region = this.regions.find(element => element.id === id);
    if (region) {
      this.regionSource.next(region);
    }
  }

  getSpotsByRegion(id: number, params: HttpParams = new HttpParams()): Observable<PaginatedModel> {
    const endpoint = this.baseUrl + 'regions/' + id + '/spots';
    const request = this.http.get<PaginatedModel>(endpoint, {params});
    request.subscribe(response => {
      this.counts = response.counts;
      this.spotsSource.next(response.data);
      this.totalSource.next(this.counts.total);
      this.pageSizeSource.next(this.counts.paging.limit);
    });
    return request;
  }
}
