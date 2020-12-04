import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatMenuModule} from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { MatTableModule } from '@angular/material/table'


import { AppRoutingModule } from './modules/routing/routing.module';


import { AppComponent } from './app.component';
import { PageAngularHelpComponent } from './components/pages/angular-help/page.angular-help';
import { BodyContentComponent } from './components/layouts/body-content/body-content.component';
import { LandingComponent } from './components/pages/landing/page.landing.component';
import { PageRegionsComponent } from './components/pages/regions/page.regions.component';
import { CenterContentComponent } from './components/layouts/center-content/center-content.component';
import { PageSpotsInRegionComponent } from './components/pages/spots-in-region/page.spots-in-region.component';
import { PageLodgmentsInSpotComponent } from './components/pages/lodgments-in-spot/page.lodgments-in-spot.component';
import { PageLodgmentComponent } from './components/pages/lodgment/page.lodgment.component';
import { PageBookingStatusComponent } from './components/pages/booking-status/page.booking-status.component';

import { SelectComponent } from './components/select/select.component';
import { SelectRegionsComponent } from './components/select-regions/select-regions.component';
import { CounterComponent } from './components/counter/counter.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { SplitVerticallyComponent } from './components/layouts/split-vertically/split-vertically.component';
import { BookingOptionsComponent } from './components/booking-options/booking-options.component';
import { HeaderBarComponent } from './components/layouts/header-bar/header-bar.component';
import {MatDialogModule} from '@angular/material/dialog';
import { BookingDetailsComponent } from './components/dialogs/booking-details/booking-details.component';
import { PageLoginComponent } from './components/pages/login/page.login.component';
import { PageUsersComponent } from './components/pages/users/page.users.component';
import {MatSortModule} from '@angular/material/sort';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { PageAddUserComponent } from './components/pages/add-user/page.add-user.component';
import { ConfirmDialogComponent } from './components/dialogs/confirm-dialog/confirm-dialog.component';
import { PageModifyUserComponent } from './components/pages/modify-user/page.modify-user.component';
import { PageBookingsComponent } from './components/pages/bookings/page.bookings.component';
import { PageModifyBookingStatusComponent } from './components/pages/modify-booking-status/page.modify-booking-status.component';
import {MatSelectModule} from '@angular/material/select';
import {MatStepperModule} from '@angular/material/stepper';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { PageLodgmentsComponent } from './components/pages/lodgments/page.lodgments.component';
import { PageLodgmentSetupComponent } from './components/pages/lodgment-setup/page.lodgment-setup.component';
import { ErrorDialogComponent } from './components/dialogs/error-dialog/error-dialog.component';
import { DashboardUploadComponent } from './components/dashboard-upload/dashboard-upload.component';
import { PageSpotsComponent } from './components/pages/spots/page.spots.component';
import { PageSpotSetupComponent } from './components/pages/spot-setup/page.spot-setup.component';
import { ReviewsComponent } from './components/reviews/reviews.component';
import { PageReportsComponent } from './components/pages/reports/page.reports.component';

@NgModule({
  declarations: [
    AppComponent,
    PageAngularHelpComponent,
    BodyContentComponent,
    LandingComponent,
    PageRegionsComponent,
    CenterContentComponent,
    PageSpotsInRegionComponent,
    PageLodgmentsInSpotComponent,
    SelectComponent,
    SelectRegionsComponent,
    CounterComponent,
    PaginationComponent,
    PageLodgmentComponent,
    SplitVerticallyComponent,
    BookingOptionsComponent,
    HeaderBarComponent,
    BookingDetailsComponent,
    PageBookingStatusComponent,
    PageLoginComponent,
    PageUsersComponent,
    PageAddUserComponent,
    ConfirmDialogComponent,
    PageModifyUserComponent,
    PageBookingsComponent,
    PageModifyBookingStatusComponent,
    PageLodgmentsComponent,
    PageLodgmentSetupComponent,
    ErrorDialogComponent,
    DashboardUploadComponent,
    PageSpotsComponent,
    PageSpotSetupComponent,
    ReviewsComponent,
    PageReportsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatSidenavModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    MatInputModule,
    MatGridListModule,
    MatCardModule,
    MatPaginatorModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatMenuModule,
    MatDividerModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MatDialogModule,
    MatTableModule,
    MatSortModule,
    MatSnackBarModule,
    MatSelectModule,
    MatStepperModule,
    MatSlideToggleModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  schemas: [NO_ERRORS_SCHEMA]
})
export class AppModule { }
