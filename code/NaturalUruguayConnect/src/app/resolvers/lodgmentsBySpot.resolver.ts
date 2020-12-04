import { Injectable } from '@angular/core';

import { Resolve, ActivatedRouteSnapshot } from '@angular/router';

import { Observable } from 'rxjs';
import {PaginatedModel} from '../models/paginatedModel';
import { SpotsService} from '../services/spots/spots.service';
import {HttpParams} from '@angular/common/http';
import {RouterService} from '../services/router/router.service';
import {DateService} from '../services/date/date.service';

@Injectable({ providedIn: 'root' })
export class LodgmentsBySpotResolver implements Resolve<any> {
  constructor(private spotsService: SpotsService, private routerService: RouterService, private dateService: DateService) {}

  private getDefaultHttpParams(rawCheckIn: string, rawCheckOut: string): HttpParams {
    const checkIn = this.dateService.getDate(rawCheckIn);
    const checkOut = this.dateService.getDate(rawCheckOut);
    const params = this.routerService.buildCheckInOutParams(checkIn, checkOut);
    return params;
  }

  resolve(route: ActivatedRouteSnapshot) {
    const idParam = this.routerService.idParam;
    const checkInParam = this.routerService.checkInParam;
    const checkOutParam = this.routerService.checkOutParam;
    const id = Number(route.paramMap.get(idParam));
    const checkIn = route.queryParamMap.get(checkInParam);
    const checkOut = route.queryParamMap.get(checkOutParam);
    const params = this.getDefaultHttpParams(checkIn, checkOut);
    this.spotsService.getLodgments(id, params);
  }
}
