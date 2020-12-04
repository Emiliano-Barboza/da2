import {Component, Input, OnInit} from '@angular/core';
import {Region} from '../../models/region';
import {RegionsService} from '../../services/regions/regions.service';
import {ActivatedRoute, Router} from '@angular/router';
import { Subject, Observable, BehaviorSubject } from 'rxjs';
import {takeUntil} from 'rxjs/operators';
import {BaseComponent} from '../../helpers/baseComponent';

@Component({
  selector: 'app-select-regions',
  templateUrl: './select-regions.component.html',
  styleUrls: ['./select-regions.component.css']
})
export class SelectRegionsComponent extends BaseComponent{
  searchPlaceholderText = 'Qué región visitarás?';
  filteredRegions: Observable<any>;

  @Input() selectedValue = '';

  constructor(private regionsService: RegionsService, private router: Router, private route: ActivatedRoute) {
    super();
  }

  ngOnInit() {
    this.filteredRegions = this.regionsService.regions$;
    this.filteredRegions.pipe(takeUntil(this.destroyed)).subscribe(regions => {
      if (regions.length) {
        const regionId = Number(this.route.snapshot.paramMap.get('id'));
        this.regionsService.getRegion(regionId);
      }
    });
  }

  searchSource(value): void {
    this.regionsService.filterRegionsByName(value);
  }

  navigateToRegion(region): void {
    const url = 'regions/' + region.id + '/spots';
    this.router.navigate([url]);
  }
}
