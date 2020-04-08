import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AirlineService } from 'src/services/airline.service';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/entities/user';
import { UserService } from 'src/services/user.service';
import { Airline } from 'src/app/entities/airline';
import { Flight } from 'src/app/entities/flight';

@Component({
  selector: 'app-add-flight',
  templateUrl: './add-flight.component.html',
  styleUrls: ['./add-flight.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AddFlightComponent implements OnInit {

  adminId: number;
  admin: User;
  adminsAirline: Airline;

  constructor(private route: ActivatedRoute, private userService: UserService, private airlineService: AirlineService) {
    route.params.subscribe(params => {
      this.adminId = params.id;
    });
   }

  ngOnInit(): void {
    const allUsers = this.userService.getAllUsers();
    allUsers.forEach(user => {
      if (user.id == this.adminId) {
        this.admin = user;
        return;
      }
    });

    this.airlineService.loadAllAirlines().forEach(airline => {
      if (airline.adminid === this.admin.id) {
        this.adminsAirline = airline;
      }
    });
  }

  addFlight() {
    //this.adminsAirline.flights.push(new Flight());
  }

}
