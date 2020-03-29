import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { FlightMainFormComponent } from './flight-main-form/flight-main-form.component';
import { SignupComponent } from './signup/signup.component';
import { HomeComponent } from './home/home.component';
import { CarMainFormComponent } from './car-main-form/car-main-form.component';
import { SigninComponent } from './signin/signin.component';
import { SocialNetworkComponent } from './social-network/social-network.component';
import { BecomeAHostComponent } from './home/become-a-host/become-a-host.component';
import { AirlinesComponent } from './home/airlines/airlines.component';
import { RedirectToCarsComponent } from './home/redirect-to-cars/redirect-to-cars.component';
import { BottomMenuComponent } from './bottom-menu/bottom-menu.component';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      FlightMainFormComponent,
      SignupComponent,
      HomeComponent,
      CarMainFormComponent,
      SigninComponent,
      SocialNetworkComponent,
      BecomeAHostComponent,
      AirlinesComponent,
      RedirectToCarsComponent,
      BottomMenuComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
