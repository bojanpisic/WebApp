import { Component, OnInit } from '@angular/core';
import { Trip } from 'src/app/entities/trip';
import { TripService } from 'src/services/trip.service';

@Component({
  selector: 'app-trips',
  templateUrl: './trips.component.html',
  styleUrls: ['./trips.component.scss']
})
export class TripsComponent implements OnInit {
  trips: Array<Trip>;

  constructor(private tripService: TripService) {
    this.trips = new Array<Trip>();
   }

  ngOnInit(): void {
    this.trips = this.tripService.getAllTrips();
  }

}
