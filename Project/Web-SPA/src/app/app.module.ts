import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './components/helper/nav/nav.component';
import { FlightMainFormComponent } from './components/home/flight-main-form/flight-main-form.component';
import { SignupComponent } from './components/join/signup/signup.component';
import { HomeComponent } from './components/home/home.component';
import { SigninComponent } from './components/join/signin/signin.component';
import { SocialNetworkComponent } from './components/join/social-network/social-network.component';
import { BecomeAHostComponent } from './components/home/become-a-host/become-a-host.component';
import { RedirectToComponent } from './components/home/redirect-to/redirect-to.component';
import { BottomMenuComponent } from './components/helper/bottom-menu/bottom-menu.component';
import { FooterComponent } from './components/helper/footer/footer.component';
import { DriveMainFormComponent } from './components/home/drive-main-form/drive-main-form.component';
import { FormsModule } from '@angular/forms';
import { AirlineComponent } from './components/al-components/airline/airline.component';
import { AirlinesComponent } from './components/al-components/airlines/airlines.component';
import { AirlineInfoComponent } from './components/al-components/airline-info/airline-info.component';
import { PersonNumSearchComponent } from './components/helper/person-num-search/person-num-search.component';
import { RentACarServiceComponent } from './components/rac-components/rent-a-car-service/rent-a-car-service.component';
import { RentACarServicesComponent } from './components/rac-components/rent-a-car-services/rent-a-car-services.component';
import { RentACarServiceInfoComponent } from './components/rac-components/rent-a-car-service-info/rent-a-car-service-info.component';
import { TopRatedComponent } from './components/home/top-rated/top-rated.component';
import { SortAndFilterBarComponent } from './components/helper/sort-and-filter-bar/sort-and-filter-bar.component';
import { FlightFormPartComponent } from './components/home/flight-main-form/flight-form-part/flight-form-part.component';
import { RegisterCompanyComponent } from './components/register-company/register-company.component';
import { AddFlightComponent } from './components/al-components/add-flight/add-flight.component';

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
      RedirectToComponent,
      BottomMenuComponent,
      FooterComponent,
      DriveMainFormComponent,
      AirlineComponent,
      AirlinesComponent,
      TopRatedComponent,
      AirlineInfoComponent,
      PersonNumSearchComponent,
      RentACarServiceComponent,
      RentACarServicesComponent,
      RentACarServiceInfoComponent,
      SortAndFilterBarComponent,
      FlightFormPartComponent,
      RegisterCompanyComponent,
      AddFlightComponent
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
