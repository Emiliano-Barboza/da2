import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { SpotsService} from '../services/spots/spots.service';

@Injectable({ providedIn: 'root' })
export class SpotResolver implements Resolve<any> {
  constructor(private spotsService: SpotsService) {}

  resolve(route: ActivatedRouteSnapshot) {
    const id = Number(route.paramMap.get('id'));
    this.spotsService.getSpot(id);
  }
}
