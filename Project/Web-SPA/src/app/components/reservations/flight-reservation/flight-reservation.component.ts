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
import { Passenger } from 'src/app/entities/passenger';

@Component({
  selector: 'app-flight-reservation',
  templateUrl: './flight-reservation.component.html',
  styleUrls: ['./flight-reservation.component.scss']
})
export class FlightReservationComponent implements OnInit {

  index: number;
  lastStep = false;
  pickSeats = false;
  fillInfo = false;
  pickedSeat: Seat;
  passenger: Passenger;
  invitedFriend = false;

  exitReservation = 'exitReservation';
  errorReservation = 'errorReservation';
  exit = false;
  error = false;
  blur = false;

  reservation: TripReservation;
  flights: Array<Flight>;
  mySeats: Array<{seats: Array<Seat>}>;

  arrayOfValues: Array<TripParameter>;

  showTripDetails = true;
  showOfferCar = false;
  showConfirmReservation = false;

  constructor(private activatedRoute: ActivatedRoute, private location: Location, private router: Router,
              private airlineService: AirlineService) {

    const trip = this.activatedRoute.snapshot.queryParamMap.get('trip');
    console.log('1');
    if (trip === null) {
      this.arrayOfValues = new Array<TripParameter>();
      console.log('2');
    } else {
      this.arrayOfValues = JSON.parse(trip);
      console.log('3');
    }
    console.log('4');
    this.reservation = new TripReservation();
    this.flights = new Array<Flight>();

    this.mySeats = [];

    this.index = -1;
    console.log('5' + this.arrayOfValues);
  }

  ngOnInit(): void {
    console.log('BALALA');
  }

  initialize() {
    for (const item of this.arrayOfValues) {
      const flight = this.airlineService.getFlight(item.a, item.f);
      this.flights.push(flight);

      this.reservation.flights.push(flight);
      this.reservation.seats.push(new SeatsForFlight(flight));

      this.mySeats.push({
        seats: new Array<Seat>()
      });
    }
  }

  // STEPS

  goBack() {
    if (this.index === -1) {
      this.location.back();
    }
    if (this.index === 0) {
      this.exit = true;
      this.blur = true;
    } else {
      this.index--;
      this.updateVariables();
    }
  }

  nextStep() {
    if (this.index === -1) {
      this.initialize();
    }
    if (this.validateStep()) {
      this.index++;
      this.updateVariables();
    } else {
      this.error = true;
      this.blur = true;
    }
  }

  finish() {
    if (this.validateStep()) {
      this.index++;
      this.updateVariables();
    } else {
      this.error = true;
      this.blur = true;
    }
  }

  // ACTIONS

  onPickSeat(seat: any) {
    const seatIndex = this.flights[this.index].seats.indexOf(seat);
    if (this.flights[this.index].seats[seatIndex].available) {
      this.fillInfo = true;
      this.blur = true;
      this.pickedSeat = seat;
      if (this.mySeats[this.index].seats.includes(seat)) {
        const takenSeatIndex = this.reservation.seats[this.index].seats.indexOf(seat);
        this.passenger = this.reservation.seats[this.index].seats[takenSeatIndex].passenger;
        if (this.passenger.passport === '') {
          this.invitedFriend = true;
        }
      } else {
        this.passenger = undefined;
      }
    }
  }

  addPassenger(passenger: any) {
    const seatIndex = this.flights[this.index].seats.indexOf(this.pickedSeat);
    if (passenger !== null) {
      // TREBA U BAZU ZAPISATI DA JE ZAUZETO
      this.pickedSeat.passenger = passenger;
      this.mySeats[this.index].seats.push(this.pickedSeat);
      this.reservation.seats[this.index].seats.push(this.pickedSeat);
    } else if (this.mySeats[this.index].seats.includes(this.pickedSeat)) {
      const index = this.mySeats[this.index].seats.indexOf(this.pickedSeat);
      this.mySeats[this.index].seats.splice(index, 1);
      // tslint:disable-next-line:prefer-for-of
      for (let i = 0; i < this.reservation.seats.length; i++) {
        if (this.reservation.seats[i].seats.includes(this.pickedSeat)) {
          const indexOfSeat = this.reservation.seats[i].seats.indexOf(this.pickedSeat);
          this.reservation.seats[i].seats.splice(indexOfSeat, 1);
        }
      }
    }

    this.blur = false;
    this.fillInfo = false;
    this.pickedSeat = null;
  }

  // MODALS

  onExitReservation(value: any) {
    if (value) {
      this.index--;
      this.reservation.seats.forEach(seat => {
        seat.seats = [];
      });
      this.emptyReserved();
      this.updateVariables();
    }
    this.exit = false;
    this.blur = false;
  }

  onErrorReservation(value: any) {
    this.error = false;
    this.blur = false;
  }

  updateVariables() {
    // tslint:disable-next-line:max-line-length
    this.lastStep = (this.index === this.flights.length - 1) ? true : false;
    this.showTripDetails = (this.index === -1) ? true : false;
    this.pickSeats = (this.index >= 0 && this.index < this.flights.length) ? true : false;
    this.showConfirmReservation = (this.index === this.flights.length) ? true : false;
    if (this.showConfirmReservation) {
      console.log('INDEX JE' + this.index);
      console.log(this.mySeats);
    }
  }

  // HELPERS

  emptyReserved() {
    // this.flights.forEach(flight => {
    //   flight.seats.forEach(seat => {
    //     seat.reserved = false;
    //   });
    // });
    this.mySeats = [];
  }

  validateStep() {
    if (this.index >= 0) {
      if (this.reservation.seats[this.index].seats.length === 0) {
        return false;
      }
    }
    return true;
  }

}
