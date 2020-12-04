import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, Resolve} from '@angular/router';
import {LodgmentService} from '../services/lodgment/lodgment.service';
import {RouterService} from '../services/router/router.service';

@Injectable({ providedIn: 'root' })
export class LodgmentReviews implements Resolve<any> {
  constructor(private lodgmentService: LodgmentService, private routerService: RouterService) {}

  resolve(route: ActivatedRouteSnapshot): void {
    const idParam = this.routerService.idParam;
    const id = Number(route.paramMap.get(idParam));
    this.lodgmentService.getLodgmentReviews(id);
  }
}
