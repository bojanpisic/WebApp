import { Injectable } from '@angular/core';
import { Flight } from 'src/app/entities/flight';
import { AirlineService } from './airline.service';
import { ChangeOver } from 'src/app/entities/changeOver';
import { Destination } from 'src/app/entities/destination';
import { Seat } from 'src/app/entities/seat';
import { TripId } from 'src/app/entities/trip-id';
import { Address } from 'src/app/entities/address';

@Injectable({
  providedIn: 'root'
})
export class FlightService {

  flights: Array<Flight>;

  constructor(private airlines: AirlineService) {
    this.flights = new Array<Flight>();
    this.mockedFlights();
  }

  addFlightToAirline(flight: Flight, newPrice: number) {
    alert('adding spec offer. Not implemented');
  }

  getFlightsOfSpecificAirline(airlineId: number) {
    const flights = new Array<Flight>();
    this.flights.forEach(a => {
      if (a.airlineId == airlineId) {
        flights.push(a);
      }
    });
    return flights;
  }

  getAllFlights() {
    return this.flights;
  }


  mockedFlights() {
    const f1 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '03h 40min', 12,
    [new ChangeOver('08:40', '08:10', new Address('Paris', 'France', 'PAR',0,0))], 300.00, '234T',
    new Address('Madrid', 'Spain', 'MAD',0,0), new Address('Belgrade', 'Serbia', 'BEG',0,0), '06:20', '10:00',
    [new Seat(0, '33R')]);

    const f2 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '03h 10min', 12,
    [], 200.00, '234T',
    new Address('Belgrade', 'Serbia', 'BEG',0,0), new Address( 'Barcelona', 'Spain', 'BAR',0,0), '04:00', '07:10',
    [new Seat(0, '33R')]);

    const f3 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '10h 0min', 12,
    [new ChangeOver('10:00', '09:00', new Address('Paris', 'France', 'PAR',0,0)),
    new ChangeOver('12:10', '11:20', new Address('London', 'England', 'LON',0,0))], 700.00, '234T',
    new Address('Novi Sad', 'Serbia', 'NS',0,0), new Address('New York', 'USA', 'NY',0,0), '06:00', '16:00',
    [new Seat(0, '33R')]);

    const f4 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '8h 10min', 12,
    [], 700.00, '234T',
    new Address('New York', 'USA', 'NY',0,0), new Address('Novi Sad', 'Serbia', 'NS',0,0), '08:00', '16:10',
    [new Seat(0, '33R')]);

    const f5 = new Flight( 1, new Date(Date.now()), new Date(Date.now()), '1h 10min', 12,
    [], 220.00, '234T',
    new Address('Vienna', 'Austria', 'VIE',0,0), new Address('Belgrade', 'Serbia', 'BG',0,0), '14:10', '15:20',
    [new Seat(0, '33R')]);

    const f6 = new Flight( 1, new Date(Date.now()), new Date(Date.now()), '0h 50min', 12,
    [], 200.00, '234T',
    new Address('Sofia', 'Bulgaria', 'BUG',0,0), new Address('Belgrade', 'Serbia', 'BG',0,0), '12:15', '13:05',
    [new Seat(0, '33R')]);

    this.flights.push(f1);
    this.flights.push(f2);
    this.flights.push(f3);
    this.flights.push(f4);
    this.flights.push(f5);
    this.flights.push(f6);
  }
}
