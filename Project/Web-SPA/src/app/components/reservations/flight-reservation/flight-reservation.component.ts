import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TripParameter } from 'src/app/entities/trip-parameter';
import { Location } from '@angular/common';
import { Flight } from 'src/app/entities/flight';
import { AirlineService } from 'src/services/airline.service';
import { TripReservation } from 'src/app/entities/trip-reservation';
import { Seat } from 'src/app/entities/seat';
import { SeatsForFlight } from 'src/app/entities/seats-for-flight';
import { PassengersForFlight } from 'src/app/entities/passengers-for-flight';
import { SSL_OP_SSLEAY_080_CLIENT_DH_BUG } from 'constants';

@Component({
  selector: 'app-flight-reservation',
  templateUrl: './flight-reservation.component.html',
  styleUrls: ['./flight-reservation.component.scss']
})
export class FlightReservationComponent implements OnInit {

  headerText = 'Details';
  index: number;
  currentFlightIndex: number;
  steps: Array<number>;
  lastStep = false;
  pickSeats = false;
  fillInfo = false;

  reservation: TripReservation;
  flights: Array<Flight>;

  arrayOfValues: Array<TripParameter>;

  showTripDetails = true;

  showPickSeats: Array<boolean>;
  indexPickSeats: number;
  lastPick = false;

  showInviteFriends: Array<boolean>;
  indexInviteFriends: number;
  lastFriend = false;

  showOfferCar = false;

  constructor(private activatedRoute: ActivatedRoute, private location: Location, private router: Router,
              private airlineService: AirlineService) {

    const trip = this.activatedRoute.snapshot.queryParamMap.get('trip');
    if (trip === null) {
      this.arrayOfValues = new Array<TripParameter>();
    } else {
      this.arrayOfValues = JSON.parse(trip);
    }
    this.reservation = new TripReservation();
    this.flights = new Array<Flight>();

    this.index = 0;
    this.currentFlightIndex = 0;
    this.steps = new Array<number>();
  }

  ngOnInit(): void {
    this.steps.push(0);

    for (const item of this.arrayOfValues) {
      const flight = this.airlineService.getFlight(item.a, item.f);
      this.flights.push(flight);
      this.reservation.seats.push(new SeatsForFlight(flight, new Array<Seat>()));
      this.steps.push(1);
    }
    this.flights.forEach(flight => {
      this.steps.push(2);
    });

    this.reservation.flights = this.flights;
  }

  updateVariables() {
    // tslint:disable-next-line:max-line-length
    this.headerText = (this.index === 0) ? 'Details' : (this.index > 0 && this.index / this.flights.length <= 1) ? 'Pick your seats!' : 'Fill info';
    this.lastStep = (this.index === this.steps.length - 1) ? true : false;
    this.showTripDetails = (this.steps[this.index] === 0) ? true : false;
    this.pickSeats = (this.steps[this.index] === 1) ? true : false;
    this.fillInfo = (this.steps[this.index] === 2) ? true : false;
    this.currentFlightIndex = (this.index - 1) % this.flights.length;
  }

  goBack() {
    this.index--;
    this.updateVariables();
  }

  nextStep() {
    this.index++;
    this.updateVariables();
  }

  finish() {

  }

  bookedSeat(seat: any) {
    const seatIndex = this.flights[this.currentFlightIndex].seats.indexOf(seat);
    this.flights[this.currentFlightIndex].seats[seatIndex].reserved = !this.flights[this.currentFlightIndex].seats[seatIndex].reserved;
  }

  addPassengers(passenger: any) {

  }

}
