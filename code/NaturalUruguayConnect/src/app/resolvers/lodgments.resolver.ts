import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import {LodgmentService} from '../services/lodgment/lodgment.service';

@Injectable({ providedIn: 'root' })
export class LodgmentsResolver implements Resolve<any> {
  constructor(private lodgmentService: LodgmentService) {}

  resolve() {
    this.lodgmentService.getLodgments();
  }
}
