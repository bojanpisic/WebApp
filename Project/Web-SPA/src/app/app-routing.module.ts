import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { SignupComponent } from './components/join/signup/signup.component';
import { SigninComponent } from './components/join/signin/signin.component';
import { AirlinesComponent } from './components/al-components/airlines/airlines.component';
import { AirlineInfoComponent } from './components/al-components/airline-info/airline-info.component';
import { RentACarServiceInfoComponent } from './components/rac-components/rent-a-car-service-info/rent-a-car-service-info.component';
import { RentACarServicesComponent } from './components/rac-components/rent-a-car-services/rent-a-car-services.component';
import { SortAndFilterBarComponent } from './components/helper/sort-and-filter-bar/sort-and-filter-bar.component';
import { RegisterCompanyComponent } from './components/register-company/register-company.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'signup', component: SignupComponent},
  {path: 'signin', component: SigninComponent},
  {path: ':username/signin', component: SigninComponent},
  {path: 'sortfilter', component: SortAndFilterBarComponent},
  {path: ':username/:option/register-company', component: RegisterCompanyComponent},

  {
    path: 'airlines',
    children: [
      {path: ':id/airline-info', component: AirlineInfoComponent},
      {path: '', component: AirlinesComponent}
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
