import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LandingComponent } from '../../components/pages/landing/page.landing.component';
import { PageAngularHelpComponent } from '../../components/pages/angular-help/page.angular-help';
import { PageNotFoundComponent } from '../../components/pages/page-not-found/page-not-found.component';
import { PageRegionsComponent } from '../../components/pages/regions/page.regions.component';
import { PageSpotsInRegionComponent } from '../../components/pages/spots-in-region/page.spots-in-region.component';
import { PageLodgmentsInSpotComponent } from '../../components/pages/lodgments-in-spot/page.lodgments-in-spot.component';
import { PageLodgmentComponent } from '../../components/pages/lodgment/page.lodgment.component';
import { PageLodgmentsComponent } from '../../components/pages/lodgments/page.lodgments.component';

import { RegionsResolver } from '../../resolvers/regions.resolver';
import { SpotsByRegionResolver } from '../../resolvers/spotsByRegion.resolver';
import { LodgmentsBySpotResolver } from '../../resolvers/lodgmentsBySpot.resolver';
import { SpotResolver } from '../../resolvers/spot.resolver';
import { LodgmentResolver } from '../../resolvers/lodgment.resolver';
import {PageBookingStatusComponent} from '../../components/pages/booking-status/page.booking-status.component';
import {PageLoginComponent} from '../../components/pages/login/page.login.component';
import {PageUsersComponent} from '../../components/pages/users/page.users.component';
import {UsersResolver} from '../../resolvers/users.resolver';
import {PageAddUserComponent} from '../../components/pages/add-user/page.add-user.component';
import {PageModifyUserComponent} from '../../components/pages/modify-user/page.modify-user.component';
import {UserByIdResolver} from '../../resolvers/userById.resolver';
import {PageBookingsComponent} from '../../components/pages/bookings/page.bookings.component';
import {BookingsResolver} from '../../resolvers/bookings.resolver';
import {PageModifyBookingStatusComponent} from '../../components/pages/modify-booking-status/page.modify-booking-status.component';
import {BookingsByConfirmationCodeResolver} from '../../resolvers/bookingByConfirmationCode.resolver';
import {LodgmentsResolver} from '../../resolvers/lodgments.resolver';
import {PageLodgmentSetupComponent} from '../../components/pages/lodgment-setup/page.lodgment-setup.component';
import {PageSpotsComponent} from '../../components/pages/spots/page.spots.component';
import {SpotsResolver} from '../../resolvers/spots.resolver';
import {PageSpotSetupComponent} from '../../components/pages/spot-setup/page.spot-setup.component';
import {LodgmentReviews} from '../../resolvers/lodgmentReviews.resolver';
import {PageReportsComponent} from '../../components/pages/reports/page.reports.component';


const appRoutes: Routes = [
  {
    path: '',
    component: LandingComponent,
    children: [
      { path: '', redirectTo: 'regions', pathMatch: 'full' },
      {
        path: 'regions',
        component: PageRegionsComponent,
        resolve: { regions: RegionsResolver }
      },
      {
        path: 'regions/:id/spots',
        component: PageSpotsInRegionComponent,
        resolve: {
          spots: SpotsByRegionResolver,
          regions: RegionsResolver
        }
      },
      {
        path: 'spots',
        component: PageSpotsComponent,
        resolve: {
          spots: SpotsResolver
        }
      },
      {
        path: 'spots/create',
        component: PageSpotSetupComponent
      },
      {
        path: 'spots/:id/lodgments',
        component: PageLodgmentsInSpotComponent,
        resolve: {
          spot: SpotResolver,
          spots: LodgmentsBySpotResolver
        }
      },
      {
        path: 'lodgments',
        component: PageLodgmentsComponent,
        resolve: {
          spots: LodgmentsResolver
        }
      },
      {
        path: 'lodgments/create',
        component: PageLodgmentSetupComponent
      },
      {
        path: 'lodgments/:id',
        component: PageLodgmentComponent,
        resolve: {
          lodgment: LodgmentResolver,
          reviews: LodgmentReviews
        }
      },
      {
        path: 'booking-status',
        component: PageBookingStatusComponent
      },
      {
        path: 'bookings',
        component: PageBookingsComponent,
        resolve: {
          spots: BookingsResolver
        }
      },
      {
        path: 'bookings/:confirmationCode/status',
        component: PageModifyBookingStatusComponent,
        resolve: {
          spots: BookingsByConfirmationCodeResolver
        }
      },
      {
        path: 'reports',
        component: PageReportsComponent,
        resolve: {
          spots: RegionsResolver
        }
      },
      {
        path: 'users',
        component: PageUsersComponent,
        resolve: {
          spots: UsersResolver
        }
      },
      {
        path: 'users/create',
        component: PageAddUserComponent
      },
      {
        path: 'users/:id/modify',
        component: PageModifyUserComponent,
        resolve: {
          spots: UserByIdResolver
        }
      },
    ]
  },
  {
    path: 'login',
    component: PageLoginComponent
  },
  {
    path: 'angular-help',
    component: PageAngularHelpComponent
  },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: false } // <-- debugging purposes only
    )
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {}
