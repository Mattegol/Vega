import * as Raven from 'raven-js';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule, BrowserXhr } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ToastyModule } from 'ng2-toasty';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { ChartsModule } from 'ng2-charts';

import { VehicleService } from './services/vehicle.service';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { VehicleFormComponent } from './vehicle-form/vehicle-form.component';
import { AppErrorHandler } from './app.error-handler';
import { VehicleListComponent } from './vehicle-list/vehicle-list.component';
import { PaginationComponent } from './Shared/pagination.component';
import { ViewVehicleComponent } from './view-vehicle/view-vehicle.component';
import { PhotoService } from './services/photo.service';
import { BrowserXhrWithProgress, ProgressService } from './services/progress.service';
import { AuthService } from './services/auth.service';
import { ProfileComponent } from './profile/profile.component';
import { AdminComponent } from './admin/admin.component';
import { AuthGuardService } from './services/auth-guard.service';
import { ScopeGuardService } from './services/scope-guard.service';

Raven
  .config('https://d1b0b1308f474d258b520352c4f37bc4@sentry.io/1455062')
  .install();

@NgModule({
   declarations: [
      AppComponent,
      NavMenuComponent,
      HomeComponent,
      CounterComponent,
      FetchDataComponent,
      VehicleFormComponent,
      VehicleListComponent,
      PaginationComponent,
      ViewVehicleComponent,
      ProfileComponent,
      AdminComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      HttpModule,
      FormsModule,
      AngularFontAwesomeModule,
      ChartsModule,
      ToastyModule.forRoot(),
      RouterModule.forRoot([
         { path: '', redirectTo: 'vehicles', pathMatch: 'full' },
         { path: 'vehicles/new', component: VehicleFormComponent, canActivate: [ AuthGuardService ] },
         { path: 'vehicles/edit/:id', component: VehicleFormComponent, canActivate: [ AuthGuardService ] },
         { path: 'vehicles/:id', component: ViewVehicleComponent },
         { path: 'vehicles', component: VehicleListComponent },
         { path: 'admin', component: AdminComponent, canActivate: [ ScopeGuardService ], data: { expectedScopes: ['write:admin']} },
         { path: 'profile', component: ProfileComponent, canActivate: [ AuthGuardService ] },
         { path: 'home', component: HomeComponent },
         { path: '**', redirectTo: 'home' }
      ])
   ],
   providers: [
      { provide: ErrorHandler, useClass: AppErrorHandler },
      { provide: BrowserXhr, useClass: BrowserXhrWithProgress },
      VehicleService,
      AuthService,
      PhotoService,
      ProgressService,
      AuthGuardService,
      ScopeGuardService
   ],
      bootstrap: [AppComponent]
})
export class AppModule { }
