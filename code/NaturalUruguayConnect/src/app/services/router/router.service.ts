import { Injectable } from '@angular/core';
import {Router, ActivatedRoute, Params, ActivatedRouteSnapshot} from '@angular/router';
import {HttpParams} from '@angular/common/http';
import {DateService} from '../date/date.service';
import {PageEvent} from '@angular/material/paginator';
import {PagingModel} from '../../models/pagingModel';

@Injectable({
  providedIn: 'root'
})
export class RouterService {
  readonly checkInParam = 'checkIn';
  readonly checkOutParam = 'checkOut';
  readonly startDateParam = 'startDate';
  readonly endDateParam = 'endDate';
  readonly offsetParam = 'offset';
  readonly limitParam = 'limit';
  readonly directionParam = 'direction';
  readonly orderParam = 'order';
  readonly filterByParam = 'filterBy';
  readonly amountOfAdultsParam = 'amountOfAdults';
  readonly amountOfUnderAgeParam = 'amountOfUnderAge';
  readonly amountOfBabiesParam = 'amountOfBabies';
  readonly amountOfVeteransParam = 'amountOfVeterans';
  readonly idParam = 'id';
  readonly confirmationCodeParam = 'confirmationCode';
  readonly nameParam = 'name';


  constructor(private router: Router, private route: ActivatedRoute, private dateService: DateService) { }

  private intanceHttpParams(params: HttpParams = null): HttpParams {
    if (params == null) {
      params = new HttpParams();
    }
    return params;
  }

  buildQueryParams(httpParams: HttpParams): Params {
    const params: Params = {};
    httpParams = this.intanceHttpParams(httpParams);
    httpParams.keys().forEach((key) => {
      if (key === this.checkInParam || key === this.checkOutParam) {
        params[key] = this.dateService.convertTicksToDateString(httpParams.get(key));
      } else {
        params[key] = httpParams.get(key);
      }
    });
    return params;
  }
  buildPageParams(pageEvent, params: HttpParams = null): HttpParams {
    params = this.intanceHttpParams(params);
    if (pageEvent) {
      const offset = pageEvent.pageIndex * pageEvent.pageSize;
      params = params.append(this.offsetParam, offset.toString());
      params = params.append(this.limitParam, pageEvent.pageSize);
    }
    return params;
  }
  buildDirectionParams(direction: string, order: string, params: HttpParams = null): HttpParams {
    params = this.intanceHttpParams(params);
    if (direction) {
      params = params.append(this.directionParam, direction);
    }
    if (order) {
      params = params.append(this.orderParam, order);
    }
    return params;
  }
  buildCheckInOutParams(checkIn: Date, checkOut: Date, params: HttpParams = null): HttpParams {
    params = this.intanceHttpParams(params);
    const convertedCheckIn = this.dateService.converToTicks(checkIn);
    const convertedCheckOut = this.dateService.converToTicks(checkOut);
    params = params.append(this.checkInParam, convertedCheckIn.toString());
    params = params.append(this.checkOutParam, convertedCheckOut.toString());
    return params;
  }

  buildReportParams(name: string, checkIn: Date, checkOut: Date, params: HttpParams = null): HttpParams {
    params = this.intanceHttpParams(params);
    const convertedCheckIn = this.dateService.converToTicks(checkIn);
    const convertedCheckOut = this.dateService.converToTicks(checkOut);
    params = params.append(this.startDateParam, convertedCheckIn.toString());
    params = params.append(this.endDateParam, convertedCheckOut.toString());
    params = params.append(this.nameParam, name);
    return params;
  }

  buildPagingParams(paginModel: PagingModel): HttpParams {
    let params = this.intanceHttpParams();
    if (paginModel.offset) {
      params = params.append(this.offsetParam, paginModel.offset.toString());
    }
    if (paginModel.limit) {
      params = params.append(this.limitParam, paginModel.limit.toString());
    }
    if (paginModel.direction) {
      params = params.append(this.directionParam, paginModel.direction);
    }
    if (paginModel.order) {
      params = params.append(this.orderParam, paginModel.order);
    }
    if (paginModel.filterBy) {
      params = params.append(this.filterByParam, paginModel.filterBy);
    }
    return params;
  }

  getPagingModel(direction: string, order: string, filterBy: string, pageEvent: PageEvent): PagingModel {
    const pagingModel: PagingModel = {
      direction: direction || '',
      offset: pageEvent ? pageEvent.pageIndex * pageEvent.pageSize : 0,
      limit: pageEvent ? pageEvent.pageSize : 30,
      filterBy: filterBy ? filterBy.trim() : '',
      order: order || '',
    };

    return pagingModel;
  }

  refreshQueryParams(queryParams: Params): void {
    this.router.navigate([], {
      queryParams: queryParams,
      queryParamsHandling: 'merge'
    });
  }
  navigateTo(url: string, queryParams?: Params): void {
    this.router.navigate([url], {
      queryParams: queryParams,
      queryParamsHandling: 'merge'
    });
  }
}
