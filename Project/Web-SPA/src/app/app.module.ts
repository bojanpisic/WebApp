import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { FlightMainFormComponent } from './home/flight-main-form/flight-main-form.component';
import { SignupComponent } from './signup/signup.component';
import { HomeComponent } from './home/home.component';
import { SigninComponent } from './signin/signin.component';
import { SocialNetworkComponent } from './social-network/social-network.component';
import { BecomeAHostComponent } from './home/become-a-host/become-a-host.component';
import { RedirectToCarsComponent } from './home/redirect-to-cars/redirect-to-cars.component';
import { BottomMenuComponent } from './bottom-menu/bottom-menu.component';
import { FooterComponent } from './footer/footer.component';
import { DriveMainFormComponent } from './home/drive-main-form/drive-main-form.component';
import { FormsModule } from '@angular/forms';
import { RedirectToAirlinesComponent } from './home/redirect-to-airlines/redirect-to-airlines.component';
import { ShowRentACarServicesComponent } from './home/show-rent-a-car-services/show-rent-a-car-services.component';
import { AirlineInfoComponent } from './airline-info/airline-info.component';
import { AirlinesListComponent } from './airlines-list/airlines-list.component';
import { AirlinesHomeComponent } from './home/airlines-home/airlines-home.component';
import { AirlinesComponent } from './airlines/airlines.component';
import { PersonNumSearchComponent } from './home/person-num-search/person-num-search.component';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      FlightMainFormComponent,
      SignupComponent,
      HomeComponent,
      SigninComponent,
      SocialNetworkComponent,
      BecomeAHostComponent,
      RedirectToCarsComponent,
      BottomMenuComponent,
      FooterComponent,
      DriveMainFormComponent,
      RedirectToAirlinesComponent,
      ShowRentACarServicesComponent,
      AirlineInfoComponent,
      AirlinesListComponent,
      AirlinesHomeComponent,
      AirlinesComponent,
      PersonNumSearchComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      FormsModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
