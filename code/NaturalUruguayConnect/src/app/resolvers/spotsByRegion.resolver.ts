import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import {RegionsService} from '../services/regions/regions.service';

@Injectable({ providedIn: 'root' })
export class SpotsByRegionResolver implements Resolve<any> {
  constructor(private regionsService: RegionsService) {}

  resolve(route: ActivatedRouteSnapshot) {
    const id = Number(route.paramMap.get('id'));
    this.regionsService.getSpotsByRegion(id);
  }
}
