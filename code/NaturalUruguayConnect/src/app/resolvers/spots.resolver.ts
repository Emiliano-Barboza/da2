import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import {SpotsService} from '../services/spots/spots.service';

@Injectable({ providedIn: 'root' })
export class SpotsResolver implements Resolve<any> {
  constructor(private spotsService: SpotsService) {}

  resolve() {
    this.spotsService.getSpots();
  }
}
