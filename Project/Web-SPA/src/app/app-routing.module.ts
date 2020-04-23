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
import { AddFlightComponent } from './components/al-components/add-flight/add-flight.component';
import { SpecialOffersComponent } from './components/al-components/special-offers/special-offers.component';
import { FilterComponent } from './components/al-components/filter/filter.component';
import { AirlinesHeaderComponent } from './components/al-components/airlines/airlines-header/airlines-header.component';
import { TripsComponent } from './components/al-components/trips/trips.component';
import { FriendsListComponent } from './components/registered-user/friends-list/friends-list.component';
import { CarsComponent } from './components/rac-components/cars/cars.component';
import { ProfileComponent } from './components/helper/profile/profile.component';
import { ShowFlightsComponent } from './components/registered-user/show-flights/show-flights.component';
import { EditProfileComponent } from './components/helper/profile/edit-profile/edit-profile.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: ':id/home', component: HomeComponent},
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
  {path: ':id/friends-list', component: FriendsListComponent},
  // {path: ':id/profile', component: ProfileComponent},
  {
    path: ':id/profile',
    children: [
      {path: '', component: ProfileComponent},
      {path: 'edit-profile', component: EditProfileComponent}
    ]
  },
  {path: ':id/flights', component: ShowFlightsComponent},

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
