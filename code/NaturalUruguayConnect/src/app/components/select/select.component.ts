import {Component, ElementRef, OnInit, ViewChild, Input,  Output, EventEmitter} from '@angular/core';
import {FormControl} from '@angular/forms';
import {Observable} from 'rxjs';
import {MatAutocompleteTrigger} from '@angular/material/autocomplete';
import {RegionsService} from '../../services/regions/regions.service';
import { Selectable } from '../../models/selectable';
import {SpotsService} from '../../services/spots/spots.service';
import {RouterService} from '../../services/router/router.service';

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.css']
})
export class SelectComponent implements OnInit {
  private timeoutID: number;
  searchControl = new FormControl();
  source: any[];
  invalidLabel: 'Seleccionar un valor';

  @Input() searchPlaceholderText = 'Place holder';
  @Input() selectedValue = '';
  @Input() searchSource: (args: any) => any[];
  @Input() filteredOptions: Observable<Selectable>;
  @Input() searchButtonVisible = true;
  @Input() appearance = '';
  @Output() selectedOptionEvent = new EventEmitter<any>();
  @ViewChild('autocompleteInput') autocompleteInput: ElementRef;

  // TODO: Improve this, check how remove this dependency since removing will break filteredOptions
  constructor(private regionsService: RegionsService, private spotService: SpotsService,
              private routerService: RouterService) {}

  private cancelTimeout(): void {
    window.clearTimeout(this.timeoutID);
  }

  ngOnInit() {
    this.searchControl.valueChanges.subscribe( value => {
      this.cancelTimeout();
      const self = this;
      this.timeoutID = window.setTimeout(function() {
        self.searchSource(value);
        }, 500);
    });
    if (this.selectedValue) {
      this.searchControl.setValue( this.selectedValue);
    }
  }

  openAutocompletePanel(event: Event, trigger: MatAutocompleteTrigger): void{
    event.stopPropagation();
    trigger.openPanel();
    this.searchControl.setValue(' ');
    this.searchControl.setValue('');
    this.autocompleteInput.nativeElement.blur();
    this.autocompleteInput.nativeElement.focus();
  }

  selectedOption(option): void {
    this.selectedOptionEvent.emit(option);
  }
}
