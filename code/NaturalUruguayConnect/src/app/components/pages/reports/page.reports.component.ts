import { Component, OnInit } from '@angular/core';
import {Lodgment} from '../../../models/lodgments';
import {SelectionModel} from '@angular/cdk/collections';
import {Observable} from 'rxjs';
import {BookingStatus} from '../../../models/bookingStatus';
import {RegionsService} from '../../../services/regions/regions.service';
import {Region} from '../../../models/region';
import {LodgmentsBookingsReport} from '../../../models/lodgmentsBookingsReport';
import {FormControl, FormGroup} from '@angular/forms';
import {Spot} from '../../../models/spot';
import {SpotsService} from '../../../services/spots/spots.service';
import {take} from 'rxjs/operators';
import {ModalService} from '../../../services/modal/modal.service';
import {RouterService} from '../../../services/router/router.service';
import {AuthService} from '../../../services/auth/auth.service';

@Component({
  selector: 'app-reports',
  templateUrl: './page.reports.component.html',
  styleUrls: ['./page.reports.component.css']
})
export class PageReportsComponent implements OnInit {

  justifyContentHeader = 'flex-start';
  regionPlaceHolder = 'Selecciona una región';
  spotsPlaceHolder = 'Selecciona un punto turístico';
  createReportLabel = 'Crear reporte';
  displayedColumns: string[] = ['name', 'count'];
  columnsToDisplay: string[] = this.displayedColumns.slice();
  lodgments: Lodgment[];
  selection = new SelectionModel<LodgmentsBookingsReport>(false, []);
  regionsOptions: Observable<Region[]>;
  spotsOptions: Observable<Spot[]>;
  reportsOptions: string[] = ['lodgments-bookings'];
  range: FormGroup;
  checkInDefault = new Date();
  checkOutDefault = new Date();
  datesText = 'Elige las fechas del reporte';
  checkInText = 'Inicio';
  checkOutText = 'Fin';
  regionId: number;
  spotId: number;
  spotsDisabled = true;
  createReportDisabled = true;
  reports: LodgmentsBookingsReport[];

  constructor(private regionsService: RegionsService, private spotsService: SpotsService,
              private modalService: ModalService, private routerService: RouterService,
              private authService: AuthService) { }

  ngOnInit(): void {
    // TODO: Move to Guard
    if (!this.authService.checkAuthenticated()) {
      this.routerService.navigateTo('/');
    }
    this.regionsOptions = this.regionsService.regions$;
    this.spotsOptions = this.regionsService.spots$;
    this.range = new FormGroup({
      checkIn: new FormControl(this.checkInDefault),
      checkOut: new FormControl(this.checkOutDefault)
    });
  }

  sortBy(): void {
    // this.search();
  }

  createReport(): void {
    const params = this.routerService.buildReportParams(this.reportsOptions[0], this.range.value.checkIn, this.range.value.checkOut);

    this.spotsService.getSpotReport(this.spotId, params).pipe(take(1)).subscribe(
      (reportResponse) => {
        this.reports = reportResponse.data;
      },
      (dataError) => {
        const message = dataError.status !== 500 ? dataError.error.detail : 'Hubo un error al crear el reporte';
        this.modalService.openErrorSnackBar(message);
      }
    );
  }

  selectedRegion(regionId): void {
    this.regionId = regionId;
    this.spotsDisabled = false;
    this.regionsService.getSpotsByRegion(this.regionId);
  }

  selectedSpot(spotId): void {
    this.spotId = spotId;
    this.createReportDisabled = false;
  }
}
