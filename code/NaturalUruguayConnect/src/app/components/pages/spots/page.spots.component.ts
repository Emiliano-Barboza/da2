import {Component, ViewChild} from '@angular/core';
import {BaseComponent} from '../../../helpers/baseComponent';
import {SelectionModel} from '@angular/cdk/collections';
import {MatSort} from '@angular/material/sort';
import {RouterService} from '../../../services/router/router.service';
import {takeUntil} from 'rxjs/operators';
import {SpotsService} from '../../../services/spots/spots.service';
import {Spot} from '../../../models/spot';
import {AuthService} from '../../../services/auth/auth.service';

@Component({
  selector: 'app-spots',
  templateUrl: './page.spots.component.html',
  styleUrls: ['./page.spots.component.css']
})
export class PageSpotsComponent extends BaseComponent {

  addSpotLabel = 'Agregar';
  justifyContentHeader = 'flex-end';
  filterByPlaceholder = 'Filtrar por puntos turísticos';
  filterBy = '';
  total: number;
  pageSize: number;
  displayedColumns: string[] = ['id', 'name', 'description'];
  columnsToDisplay: string[] = this.displayedColumns.slice();
  spots: Spot[];
  selection = new SelectionModel<Spot>(false, []);
  optionsDisabled: boolean;

  @ViewChild(MatSort) sort: MatSort;

  constructor(private spotsService: SpotsService, private routerService: RouterService,
              private authService: AuthService) {
    super();
  }

  ngOnInit(): void {
    // TODO: Move to Guard
    if (!this.authService.checkAuthenticated()) {
      this.routerService.navigateTo('/');
    }
    this.optionsDisabled = true;
    this.spotsService.spots$.pipe(takeUntil(this.destroyed)).subscribe(spots => this.spots = spots);
    this.spotsService.total$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.total = x;
    });
    this.spotsService.pageSize$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.pageSize = x;
    });
  }

  filterByEvent(filterBy): void {
    this.filterBy = filterBy;
    this.search();
  }

  search(pageEvent = null): void {
    const pagingModel = this.routerService.getPagingModel(this.sort.direction, this.sort.active, this.filterBy, pageEvent);
    const params = this.routerService.buildPagingParams(pagingModel);
    const queryParams = this.routerService.buildQueryParams(params);
    this.spotsService.getSpots(params);
    this.routerService.refreshQueryParams(queryParams);
  }

  sortBy(): void {
    this.search();
  }

  createSpot(): void {
    const url = 'spots/create';
    this.routerService.navigateTo(url);
  }

  pageChangeEvent(pageEvent): void {
    this.search(pageEvent);
  }

}
