import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import {RegionsService} from '../services/regions/regions.service';

@Injectable({ providedIn: 'root' })
export class RegionsResolver implements Resolve<any> {
  constructor(private regionsService: RegionsService) {}

  resolve() {
    this.regionsService.getRegions();
  }
}
