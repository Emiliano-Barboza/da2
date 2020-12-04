import {Component, ViewChild} from '@angular/core';
import {User} from '../../../models/user';
import {SelectionModel} from '@angular/cdk/collections';
import {MatSort} from '@angular/material/sort';
import {UserService} from '../../../services/users/user.service';
import {RouterService} from '../../../services/router/router.service';
import {ModalService} from '../../../services/modal/modal.service';
import {takeUntil} from 'rxjs/operators';
import {MatDialogConfig} from '@angular/material/dialog';
import {ConfirmDialogComponent} from '../../dialogs/confirm-dialog/confirm-dialog.component';
import {BaseComponent} from '../../../helpers/baseComponent';
import {Booking} from '../../../models/booking';
import {BookingService} from '../../../services/booking/booking.service';

@Component({
  selector: 'app-bookings',
  templateUrl: './page.bookings.component.html',
  styleUrls: ['./page.bookings.component.css']
})
export class PageBookingsComponent extends BaseComponent {

  modifyBookingLabel = 'Modificar estado';
  justifyContentHeader = 'flex-end';
  filterByPlaceholder = 'Filtrar por reservas';
  filterBy = '';
  total: number;
  pageSize: number;
  displayedColumns: string[] = ['id', 'name', 'lastName', 'email', 'checkIn', 'checkOut', 'amountGuest', 'price', 'confirmationCode', 'statusDescription'];
  columnsToDisplay: string[] = this.displayedColumns.slice();
  bookings: Booking[];
  selection = new SelectionModel<Booking>(false, []);
  optionsDisabled: boolean;

  @ViewChild(MatSort) sort: MatSort;

  constructor(private bookingService: BookingService, private routerService: RouterService) {
    super();
  }

  ngOnInit(): void {
    this.optionsDisabled = true;
    this.bookingService.bookings$.pipe(takeUntil(this.destroyed)).subscribe(bookings => this.bookings = bookings);
    this.bookingService.total$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.total = x;
    });
    this.bookingService.pageSize$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.pageSize = x;
    });
    this.selection.changed.pipe(takeUntil(this.destroyed)).subscribe(() => {
      this.optionsDisabled = !this.selection.hasValue();
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
    this.bookingService.getBookings(params);
    this.routerService.refreshQueryParams(queryParams);
  }

  sortBy(): void {
    this.search();
  }

  modifyBookingStatus(): void {
    const url = 'bookings/' + this.selection.selected[0].confirmationCode +  '/status';
    this.routerService.navigateTo(url);
  }

  pageChangeEvent(pageEvent): void {
    this.search(pageEvent);
  }
}
