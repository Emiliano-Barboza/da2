import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {PageEvent} from '@angular/material/paginator';

@Component({
  selector: 'app-counter',
  templateUrl: './counter.component.html',
  styleUrls: ['./counter.component.css']
})
export class CounterComponent implements OnInit {
  counter: number;
  isRemoveDisabled: boolean;

  @Input() placeholderText = 'Place holder';
  @Input() hintText = 'hint here';
  @Output() count = new EventEmitter<number>();

  constructor() {}

  private emitCounter(): void {
    this.count.emit(this.counter);
  }

  ngOnInit(): void {
    this.counter = 0;
    this.isRemoveDisabled = true;
  }

  add(event): void {
    event.stopPropagation();
    this.counter++;
    this.emitCounter();
    this.isRemoveDisabled = false;
  }

  remove(event): void {
    event.stopPropagation();
    if (this.counter > 0) {
      this.counter--;
      this.emitCounter();
    }
    this.isRemoveDisabled = this.counter === 0;
  }
}
