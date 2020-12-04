import {Component, ElementRef, ViewChild} from '@angular/core';
import {RegionsService} from '../../../services/regions/regions.service';
import {ActivatedRoute, Router} from '@angular/router';
import {Observable} from 'rxjs';
import { Region } from '../../../models/region';
import {BaseComponent} from '../../../helpers/baseComponent';
import {takeUntil} from 'rxjs/operators';
import {RouterService} from '../../../services/router/router.service';

@Component({
  selector: 'app-spots-in-region',
  templateUrl: './page.spots-in-region.component.html',
  styleUrls: ['./page.spots-in-region.component.css']
})
export class PageSpotsInRegionComponent extends BaseComponent {
  filteredSpots: Observable<any>;
  total: number;
  pageSize: number;
  region: Region;
  filterByPlaceholder = 'Filtrar por puntos turísticos';
  filterBy = '';

  @ViewChild('autocompleteInput') autocompleteInput: ElementRef;

  constructor(private regionsService: RegionsService, private router: Router,
              private routerService: RouterService, private route: ActivatedRoute) {
    super();
  }

  ngOnInit() {
    this.filteredSpots = this.regionsService.spots$;
    this.regionsService.region$.subscribe(region => this.region = region);
    this.regionsService.total$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.total = x;
    });
    this.regionsService.pageSize$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.pageSize = x;
    });
  }

  filterByEvent(filterBy): void {
    this.filterBy = filterBy;
    this.search();
  }

  search(pageEvent = null): void {
    const pagingModel = this.routerService.getPagingModel('', '', this.filterBy, pageEvent);
    const params = this.routerService.buildPagingParams(pagingModel);
    const queryParams = this.routerService.buildQueryParams(params);
    this.regionsService.getSpotsByRegion(this.region.id, params);
    this.routerService.refreshQueryParams(queryParams);
  }

  pageChangeEvent(pageEvent): void {
    this.search(pageEvent);
  }

  navigateToSpot(spot): void {
    const url = 'spots/' + spot.id + '/lodgments';
    this.router.navigate([url]);
  }
}
