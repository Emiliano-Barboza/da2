import { Injectable } from '@angular/core';

import { Resolve, ActivatedRouteSnapshot } from '@angular/router';

import { Lodgment } from '../models/lodgments';
import { LodgmentService} from '../services/lodgment/lodgment.service';
import {HttpParams} from '@angular/common/http';
import {RouterService} from '../services/router/router.service';
import {DateService} from '../services/date/date.service';

@Injectable({ providedIn: 'root' })
export class LodgmentResolver implements Resolve<Lodgment> {
  constructor(private lodgmentService: LodgmentService, private routerService: RouterService, private dateService: DateService) {}

  private getDefaultHttpParams(route: ActivatedRouteSnapshot): HttpParams {
    const rawCheckIn = route.queryParamMap.get('checkIn');
    const rawCheckOut = route.queryParamMap.get('checkOut');
    const checkIn = this.dateService.getDate(rawCheckIn);
    const checkOut = this.dateService.getDate(rawCheckOut);
    const params = this.routerService.buildCheckInOutParams(checkIn, checkOut);
    return params;
  }

  resolve(route: ActivatedRouteSnapshot) {

    const id = Number(route.paramMap.get('id'));
    const params = this.getDefaultHttpParams(route);
    const lodgments = this.lodgmentService.getLodgment(id, params);
    return lodgments;
  }
}
