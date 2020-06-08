import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
// import { HttpParams } from '@angular/common/http';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';


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
import { FlightFormPartComponent } from './components/home/flight-main-form/flight-form-part/flight-form-part.component';
import { RegisterCompanyComponent } from './components/register-company/register-company.component';
import { AddFlightComponent } from './components/airline-admin/airline-flights/add-flight/add-flight.component';
import { DestinationsComponent } from './components/helper/destinations/destinations.component';
import { SpecialOffersComponent } from './components/al-components/special-offers/special-offers.component';
import { PlacesSearchComponent } from './components/helper/places-search/places-search.component';
import { MapComponent } from './components/helper/map/map.component';

import { AgmCoreModule } from '@agm/core';
import { PlacesPickerComponent } from './components/helper/places-picker/places-picker.component';
import { SpecialOfferComponent } from './components/al-components/special-offers/special-offer/special-offer.component';
import { FilterComponent } from './components/al-components/filter/filter.component';
import { AirlinesHeaderComponent } from './components/al-components/airlines/airlines-header/airlines-header.component';
import { TripsComponent } from './components/al-components/trips/trips.component';
import { TripComponent } from './components/al-components/trips/trip/trip.component';
import { SearchBarComponent } from './components/helper/search-bar/search-bar.component';
import { CarsComponent } from './components/rac-components/cars/cars.component';
import { CarComponent } from './components/rac-components/cars/car/car.component';
import { ProfileComponent } from './components/helper/profile/profile.component';
import { ShowFlightsComponent } from './components/registered-user/show-flights/show-flights.component';
import { UserNavComponent } from './components/helper/user-nav/user-nav.component';
import { MyFlightsComponent } from './components/registered-user/my-flights/my-flights.component';
import { MyCarsComponent } from './components/registered-user/my-cars/my-cars.component';
import { MessagesComponent } from './components/registered-user/messages/messages.component';
import { MyFlightComponent } from './components/registered-user/my-flights/my-flight/my-flight.component';
import { MyCarComponent } from './components/registered-user/my-cars/my-car/my-car.component';
import { MessageComponent } from './components/registered-user/messages/message/message.component';
import { MessageInfoComponent } from './components/registered-user/messages/message-info/message-info.component';
import { EditProfileComponent } from './components/helper/profile/edit-profile/edit-profile.component';
import { FriendsComponent } from './components/registered-user/friends/friends.component';
import { FindFriendPipe } from './pipes/find-friend.pipe';
import { ModalComponent } from './components/helper/modal/modal.component';
import { PickSeatsComponent } from './components/reservations/flight-reservation/pick-seats/pick-seats.component';
import { InviteFriendsComponent } from './components/reservations/flight-reservation/invite-friends/invite-friends.component';
// tslint:disable-next-line:max-line-length
import { ConfirmTripReservationComponent } from './components/reservations/flight-reservation/confirm-trip-reservation/confirm-trip-reservation';
import { OfferCarComponent } from './components/reservations/flight-reservation/offer-car/offer-car.component';
import { TripDetailsComponent } from './components/reservations/flight-reservation/trip-details/trip-details.component';
import { FlightReservationComponent } from './components/reservations/flight-reservation/flight-reservation.component';
import { Location } from '@angular/common';
import { ReservedSeatsPipe } from './pipes/reserved-seats.pipe';
import { SeatPlacementPipe } from './pipes/seat-placement.pipe';
import { AdminHomeComponent } from './components/airline-admin/admin-home/admin-home.component';
import { CompanyProfileComponent } from './components/admin/company-profile/company-profile.component';
import { AirlineFlightsComponent } from './components/airline-admin/airline-flights/airline-flights.component';
import { AirlineStatsComponent } from './components/airline-admin/airline-stats/airline-stats.component';
import { AirlineDestinationsComponent } from './components/airline-admin/airline-destinations/airline-destinations.component';
// tslint:disable-next-line:max-line-length
import { AirlineDestinationComponent } from './components/airline-admin/airline-destinations/airline-destination/airline-destination.component';
import { FlightStopComponent } from './components/airline-admin/airline-flights/add-flight/flight-stop/flight-stop.component';
import { FlightComponent } from './components/al-components/flight/flight.component';
import { FindFlightPipe } from './pipes/find-flight.pipe';
import { ConfigureSeatsComponent } from './components/airline-admin/airline-flights/configure-seats/configure-seats.component';
// tslint:disable-next-line:max-line-length
import { ConfigureSeatsModalComponent } from './components/airline-admin/airline-flights/configure-seats/configure-seats-modal/configure-seats-modal.component';
import { PlaceSeatsPipe } from './pipes/place-seats.pipe';
import { RacAdminHomeComponent } from './components/rac-admin/rac-admin-home/rac-admin-home.component';
import { RacStatsComponent } from './components/rac-admin/rac-stats/rac-stats.component';
import { RacCarsComponent } from './components/rac-admin/rac-cars/rac-cars.component';
import { RacBranchesComponent } from './components/rac-admin/rac-branches/rac-branches.component';
import { AddCarComponent } from './components/rac-admin/rac-cars/add-car/add-car.component';
import { EditCarComponent } from './components/rac-admin/rac-cars/edit-car/edit-car.component';
import { AirlineSpecialOffersComponent } from './components/airline-admin/airline-special-offers/airline-special-offers.component';
import { AddSpecialOfferComponent } from './components/airline-admin/airline-special-offers/add-special-offer/add-special-offer.component';
import { AddSeatsSpecialOfferComponent } from './components/airline-admin/airline-special-offers/add-special-offer/add-seats-special-offer/add-seats-special-offer.component';
import { UserService } from 'src/services/user.service';

import { CookieService } from 'ngx-cookie-service';

import { AuthInterceptor } from './auth/auth.interceptor';
import { SocialLoginModule, AuthServiceConfig, GoogleLoginProvider, FacebookLoginProvider, AuthService } from 'angular-6-social-login';

import { TokenInterceptor } from './auth/TokenInterceptor';

export function socialConfigs() {
  const config = new AuthServiceConfig(
    [
      {
        id: FacebookLoginProvider.PROVIDER_ID,
        provider: new FacebookLoginProvider('176053310509690')
      },
      {
        id: GoogleLoginProvider.PROVIDER_ID,  
        provider: new GoogleLoginProvider('962258612347-6kbnosm4gnu8glcm4l0ke9tlepv7a1e0.apps.googleusercontent.com')
      }
    ]
  );
  return config;
}

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
      FlightFormPartComponent,
      RegisterCompanyComponent,
      AddFlightComponent,
      DestinationsComponent,
      SpecialOffersComponent,
      PlacesSearchComponent,
      MapComponent,
      PlacesPickerComponent,
      SpecialOfferComponent,
      FilterComponent,
      AirlinesHeaderComponent,
      TripsComponent,
      TripComponent,
      SearchBarComponent,
      CarsComponent,
      CarComponent,
      ProfileComponent,
      ShowFlightsComponent,
      UserNavComponent,
      MyFlightsComponent,
      MyCarsComponent,
      MessagesComponent,
      MyFlightComponent,
      MyCarComponent,
      MessageComponent,
      MessageInfoComponent,
      EditProfileComponent,
      FriendsComponent,
      FindFriendPipe,
      ModalComponent,
      PickSeatsComponent,
      InviteFriendsComponent,
      ConfirmTripReservationComponent,
      OfferCarComponent,
      TripDetailsComponent,
      FlightReservationComponent,
      ReservedSeatsPipe,
      SeatPlacementPipe,
      AdminHomeComponent,
      CompanyProfileComponent,
      AirlineFlightsComponent,
      AirlineStatsComponent,
      AirlineDestinationsComponent,
      AirlineDestinationComponent,
      FlightStopComponent,
      FlightComponent,
      FindFlightPipe,
      ConfigureSeatsComponent,
      ConfigureSeatsModalComponent,
      PlaceSeatsPipe,
      RacAdminHomeComponent,
      RacStatsComponent,
      RacCarsComponent,
      RacBranchesComponent,
      AddCarComponent,
      EditCarComponent,
      AirlineSpecialOffersComponent,
      AddSpecialOfferComponent,
      AddSeatsSpecialOfferComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      FormsModule,
      HttpClientModule,
      ReactiveFormsModule,
      AgmCoreModule.forRoot({
         apiKey: 'AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE',
         libraries: ['places']
      }),
   ],
   providers: [
      CookieService,
      UserService,
      {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
      },
      {
        provide: HTTP_INTERCEPTORS,
        useClass: TokenInterceptor,
        multi: true,
      },
      AuthService,
      {
        provide: AuthServiceConfig,
        useFactory: socialConfigs
      },

   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
