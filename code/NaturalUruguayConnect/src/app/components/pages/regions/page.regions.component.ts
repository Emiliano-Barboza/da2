import { Component } from '@angular/core';
import {BaseComponent} from '../../../helpers/baseComponent';

@Component({
  selector: 'app-regions',
  templateUrl: './page.regions.component.html',
  styleUrls: ['./page.regions.component.css']
})
export class PageRegionsComponent extends BaseComponent {
  searchText = 'Buscar en Uruguay';

  constructor() {
    super();
  }

  ngOnInit() {}

  openAutocompletePanel(event: Event): void{
    event.stopPropagation();
  }
}
