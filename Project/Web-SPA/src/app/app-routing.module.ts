import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { SignupComponent } from './components/join/signup/signup.component';
import { SigninComponent } from './components/join/signin/signin.component';
import { AirlinesComponent } from './components/al-components/airlines/airlines.component';
import { AirlineInfoComponent } from './components/al-components/airline-info/airline-info.component';
import { RentACarServiceInfoComponent } from './components/rac-components/rent-a-car-service-info/rent-a-car-service-info.component';
import { RentACarServicesComponent } from './components/rac-components/rent-a-car-services/rent-a-car-services.component';
import { RegisterCompanyComponent } from './components/register-company/register-company.component';
import { AddFlightComponent } from './components/airline-admin/airline-flights/add-flight/add-flight.component';
import { SpecialOffersComponent } from './components/al-components/special-offers/special-offers.component';
import { FilterComponent } from './components/al-components/filter/filter.component';
import { AirlinesHeaderComponent } from './components/al-components/airlines/airlines-header/airlines-header.component';
import { TripsComponent } from './components/al-components/trips/trips.component';
import { CarsComponent } from './components/rac-components/cars/cars.component';
import { ProfileComponent } from './components/helper/profile/profile.component';
import { ShowFlightsComponent } from './components/registered-user/show-flights/show-flights.component';
import { EditProfileComponent } from './components/helper/profile/edit-profile/edit-profile.component';
import { FriendsComponent } from './components/registered-user/friends/friends.component';
import { MessagesComponent } from './components/registered-user/messages/messages.component';
import { TripDetailsComponent } from './components/reservations/flight-reservation/trip-details/trip-details.component';
import { TripParameter } from './entities/trip-parameter';
import { PickSeatsComponent } from './components/reservations/flight-reservation/pick-seats/pick-seats.component';
import { FlightReservationComponent } from './components/reservations/flight-reservation/flight-reservation.component';
import { AdminHomeComponent } from './components/airline-admin/admin-home/admin-home.component';
import { AirlineDestinationsComponent } from './components/airline-admin/airline-destinations/airline-destinations.component';
import { AirlineFlightsComponent } from './components/airline-admin/airline-flights/airline-flights.component';
import { CompanyProfileComponent } from './components/admin/company-profile/company-profile.component';
import { ConfigureSeatsComponent } from './components/airline-admin/airline-flights/configure-seats/configure-seats.component';
import { RacAdminHomeComponent } from './components/rac-admin/rac-admin-home/rac-admin-home.component';
import { RacBranchesComponent } from './components/rac-admin/rac-branches/rac-branches.component';
import { RacCarsComponent } from './components/rac-admin/rac-cars/rac-cars.component';
import { AddCarComponent } from './components/rac-admin/rac-cars/add-car/add-car.component';
import { EditCarComponent } from './components/rac-admin/rac-cars/edit-car/edit-car.component';
import { AirlineSpecialOffersComponent } from './components/airline-admin/airline-special-offers/airline-special-offers.component';
import { AddSpecialOfferComponent } from './components/airline-admin/airline-special-offers/add-special-offer/add-special-offer.component';

const routes: Routes = [
  {path: 'admin/:id',
  children: [
    {path: '', component: AdminHomeComponent},
    {path: 'destinations', component: AirlineDestinationsComponent},
    {path: 'flights',
    children : [
      {path: '', component: AirlineFlightsComponent},
      {path: 'add-flight', component: AddFlightComponent},
      {path: ':flight/configure-seats', component: ConfigureSeatsComponent},
    ]},
    {path: 'special-offers',
    children : [
      {path: '',  component: AirlineSpecialOffersComponent},
      {path: 'add-special-offer', component: AddSpecialOfferComponent},
    ]},
    {path: ':type', component: CompanyProfileComponent},
  ]},
  {path: 'rac-admin/:id',
  children: [
    {path: '', component: RacAdminHomeComponent},
    {path: 'branches', component: RacBranchesComponent},
    {path: 'cars',
    children : [
      {path: '', component: RacCarsComponent},
      {path: 'add-car', component: AddCarComponent},
      {path: ':car/edit-car', component: EditCarComponent},
    ]},
    {path: ':type', component: CompanyProfileComponent},
  ]},
  {path: '', component: HomeComponent},
  {path: ':id/home', component: HomeComponent},
  {path: ':id/cars', component: CarsComponent},
  {
    path: ':id/trips',
    children: [
      {path: '', component: TripsComponent},
      {path: 'filter', component: FilterComponent},
      {
        path: 'trip-reservation',
        children: [
          {path: '', component: FlightReservationComponent},
        ]
      }
    ]
  },
  {path: 'signup', component: SignupComponent},
  {path: 'signin', component: SigninComponent},
  {path: ':username/signin', component: SigninComponent},
  {path: ':username/:option/register-company', component: RegisterCompanyComponent},
  {
    path: 'trips',
    children: [
      {path: '', component: TripsComponent},
      {path: 'filter', component: FilterComponent}
    ]
  },
  {
    path: 'cars',
    children: [
      {path: '', component: CarsComponent},
      {path: 'filter', component: FilterComponent}
    ]
  },
  {path: 'airlines-header', component: AirlinesHeaderComponent},
  {
    path: ':id/profile',
    children: [
      {path: '', component: ProfileComponent},
      {path: 'edit-profile', component: EditProfileComponent},
      {path: 'friends', component: FriendsComponent}
    ]
  },
  {path: ':id/flights', component: ShowFlightsComponent},
  {path: ':id/messages', component: MessagesComponent},
  {
    path: 'airlines',
    children: [
      {path: '', component: AirlinesComponent},
      {
        path: ':id/airline-info',
        children: [
          {path: '', component: AirlineInfoComponent},
          {path: 'special-offers', component: SpecialOffersComponent}
        ]
      },
      {path: ':id/flight-add', component: AddFlightComponent},
    ]
  },

  {
    path: 'rent-a-car-services',
    children: [
      {path: ':id/rent-a-car-service-info', component: RentACarServiceInfoComponent},
      {path: '', component: RentACarServicesComponent}
    ]
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
