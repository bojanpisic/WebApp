import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { FlightMainFormComponent } from './home/flight-main-form/flight-main-form.component';
import { SignupComponent } from './signup/signup.component';
import { SigninComponent } from './signin/signin.component';
import { AirlineInfoComponent } from './airline-info/airline-info.component';
import { AirlinesListComponent } from './airlines-list/airlines-list.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'signup', component: SignupComponent},
  {path: 'signin', component: SigninComponent},

  {
    path: 'airlines',
    children: [
      {path: ':id/airline-info', component: AirlineInfoComponent},
      {path: 'airlines-list', component: AirlinesListComponent}
    ]
  }


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
