import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {PageEvent} from '@angular/material/paginator';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {
  private timeoutID: number;
  filterLabel = 'Filtro';
  pageSizeOptions = [10, 30, 50, 100];
  @Input() totalItems = 0;
  @Input() pageSize = 30;
  @Input() filterByPlaceholder = 'Filtrar por...';
  @Output() pageEvent = new EventEmitter<PageEvent>();
  @Output() filterBy = new EventEmitter<string>();

  constructor() { }

  private cancelTimeout(): void {
    window.clearTimeout(this.timeoutID);
  }

  private emitFilterByEvent(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    const value = filterValue.trim().toLowerCase();
    this.filterBy.emit(value);
  }

  ngOnInit(): void {}

  pageChangeEvent(pageEvent): void {
    this.pageEvent.emit(pageEvent);
  }

  filterByEvent(event: Event): void {
    this.cancelTimeout();
    const self = this;
    this.timeoutID = window.setTimeout(function() { self.emitFilterByEvent(event); }, 500);
  }
}
