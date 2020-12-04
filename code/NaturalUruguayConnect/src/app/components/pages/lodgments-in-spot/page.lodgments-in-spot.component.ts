import { Component, ViewChild } from '@angular/core';
import { SpotsService } from '../../../services/spots/spots.service';
import { RouterService } from '../../../services/router/router.service';
import { Spot } from '../../../models/spot';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import {HttpParams} from '@angular/common/http';
import {BookingOptionsComponent} from '../../booking-options/booking-options.component';
import {BaseComponent} from '../../../helpers/baseComponent';

@Component({
  selector: 'app-lodgments-in-spot.',
  templateUrl: './page.lodgments-in-spot.component.html',
  styleUrls: ['./page.lodgments-in-spot.component.css']
})
export class PageLodgmentsInSpotComponent extends BaseComponent {
  goBackUrl = '/regions/';
  searchText = 'Buscar hospedajes';
  filterByPlaceholder = 'Filtrar por hospedajes';
  spotName = '';
  regionName = '';
  filteredLodgments: Observable<any>;
  spot: Spot;
  total: number;
  pageSize: number;
  filterBy = '';

  @ViewChild(BookingOptionsComponent) bookingOptions;

  constructor(private spotsService: SpotsService, private routerService: RouterService){
    super();
  }

  private buildGuestsParams(params: HttpParams): HttpParams {
    let param = '';
    if (this.bookingOptions.amountOfAdults) {
      param = this.routerService.amountOfAdultsParam;
      params = params.append(param, this.bookingOptions.amountOfAdults.toString());
    }
    if (this.bookingOptions.amountOfUnderAge) {
      param = this.routerService.amountOfUnderAgeParam;
      params = params.append(param, this.bookingOptions.amountOfUnderAge.toString());
    }
    if (this.bookingOptions.amountOfBabies) {
      param = this.routerService.amountOfBabiesParam;
      params = params.append(param, this.bookingOptions.amountOfBabies.toString());
    }
    if (this.bookingOptions.amountOfVeterans) {
      param = this.routerService.amountOfVeteransParam;
      params = params.append(param, this.bookingOptions.amountOfVeterans.toString());
    }
    return params;
  }

  private getParams(pageEvent): HttpParams {
    const pagingModel = this.routerService.getPagingModel('', '', this.filterBy, pageEvent);
    let params = this.routerService.buildPagingParams(pagingModel);
    params = this.routerService.buildCheckInOutParams(this.bookingOptions.getCheckIn(), this.bookingOptions.getCheckOut(), params);
    params = this.buildGuestsParams(params);
    return params;
  }

  ngOnInit(): void {
    this.filteredLodgments = this.spotsService.lodgments$;
    this.spotsService.total$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.total = x;
    });
    this.spotsService.pageSize$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.pageSize = x;
    });
    this.spotsService.spot$.pipe(takeUntil(this.destroyed)).subscribe(spot => {
      if (spot) {
        this.spot = spot;
        this.spotName = spot.name;
        this.regionName = spot.region.name;
        this.goBackUrl += this.spot.region.id + '/spots';
      }
    });
  }

  search(pageEvent = null): void {
    const params = this.getParams(pageEvent);
    const queryParams = this.routerService.buildQueryParams(params);
    this.spotsService.getLodgments(this.spot.id, params);
    this.routerService.refreshQueryParams(queryParams);
  }

  navigateToLodgment(lodgment): void {
    const params = this.routerService.buildCheckInOutParams(this.bookingOptions.getCheckIn(), this.bookingOptions.getCheckOut());
    const queryParams = this.routerService.buildQueryParams(params);
    const url = 'lodgments/' + lodgment.id;
    this.routerService.navigateTo(url, queryParams);
  }

  filterByEvent(filterBy): void {
    this.filterBy = filterBy;
    this.search();
  }

  pageChangeEvent(pageEvent): void {
    this.search(pageEvent);
  }

}
