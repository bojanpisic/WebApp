import { Component, OnInit } from '@angular/core';
import { Trip } from 'src/app/entities/trip';
import { TripService } from 'src/services/trip.service';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { RegisteredUser } from 'src/app/entities/registeredUser';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-trips',
  templateUrl: './trips.component.html',
  styleUrls: ['./trips.component.scss']
})
export class TripsComponent implements OnInit {
  user: RegisteredUser;
  userId: number;
  trips: Array<Trip>;

  constructor(private userService: UserService, private tripService: TripService,
              private location: Location, private route: ActivatedRoute) {
    this.route.params.subscribe(param => {
      this.userId = param.id;
    });
    this.trips = new Array<Trip>();
   }

  ngOnInit(): void {
    if (this.userId !== undefined) {
      this.user = this.userService.getUser(this.userId);
    }
    this.trips = this.tripService.getAllTrips();
  }

  goBack() {
    this.location.back();
  }

}
