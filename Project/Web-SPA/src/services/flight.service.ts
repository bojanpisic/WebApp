import { Injectable } from '@angular/core';
import { Flight } from 'src/app/entities/flight';
import { ChangeOver } from 'src/app/entities/changeOver';
import { Destination } from 'src/app/entities/destination';
import { Seat } from 'src/app/entities/seat';
import { TripId } from 'src/app/entities/trip-id';
import { Address } from 'src/app/entities/address';
import { SeatsService } from './seats.service';

@Injectable({
  providedIn: 'root'
})
export class FlightService {

  flights: Array<Flight>;

  constructor(private seats: SeatsService) {
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

    const seats1 = this.makeSeats();
    const seats2 = this.makeSeats();
    const seats3 = this.makeSeats();
    const seats4 = this.makeSeats();
    const seats5 = this.makeSeats();
    const seats6 = this.makeSeats();

    const f1 = new Flight( 0, new Date(2020, 4, 28), new Date(Date.now()), '03h 40min', 12,
    [new ChangeOver('08:40', '08:10', new Address('Paris', 'France', 'PAR', 0, 0))], 300.00, '234A',
    new Address('Madrid', 'Spain', 'MAD', 0, 0), new Address('Belgrade', 'Serbia', 'BEG', 0, 0), '06:20', '10:00',
    [new Seat('F', 'A', 1), new Seat('F', 'B', 1), new Seat('F', 'C', 1), new Seat('F', 'D', 1), new Seat('F', 'E', 1),
    new Seat('F', 'E', 1), new Seat('F', 'F', 1), new Seat('F', 'A', 2), new Seat('F', 'B', 2), new Seat('F', 'C', 2),
    new Seat('F', 'D', 2), new Seat('F', 'E', 2), new Seat('F', 'F', 2), new Seat('E', 'A', 1), new Seat('E', 'B', 1),
    new Seat('E', 'C', 1), new Seat('B', 'A', 1), new Seat('B', 'B', 1), new Seat('B', 'C', 1), new Seat('B', 'D', 1)]);

    const f2 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '03h 10min', 12,
    [], 200.00, '234B',
    new Address('Belgrade', 'Serbia', 'BEG', 0, 0), new Address( 'Barcelona', 'Spain', 'BAR', 0, 0), '04:00', '07:10',
    seats2);

    const f3 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '10h 0min', 12,
    [new ChangeOver('10:00', '09:00', new Address('Paris', 'France', 'PAR', 0, 0)),
    new ChangeOver('12:10', '11:20', new Address('London', 'England', 'LON', 0, 0))], 700.00, '234C',
    new Address('Novi Sad', 'Serbia', 'NS', 0, 0), new Address('New York', 'USA', 'NY', 0, 0), '06:00', '16:00',
    seats3);

    const f4 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '8h 10min', 12,
    [], 700.00, '234D',
    new Address('New York', 'USA', 'NY', 0, 0), new Address('Novi Sad', 'Serbia', 'NS', 0, 0), '08:00', '16:10',
    seats4);

    const f5 = new Flight( 1, new Date(Date.now()), new Date(Date.now()), '1h 10min', 12,
    [], 220.00, 'V11T',
    new Address('Vienna', 'Austria', 'VIE', 0, 0), new Address('Belgrade', 'Serbia', 'BG', 0, 0), '14:10', '15:20',
    seats5);

    const f6 = new Flight( 1, new Date(Date.now()), new Date(Date.now()), '0h 50min', 12,
    [], 200.00, 'V11R',
    new Address('Sofia', 'Bulgaria', 'BUG', 0, 0), new Address('Belgrade', 'Serbia', 'BG', 0, 0), '12:15', '13:05',
    seats6);

    this.flights.push(f1);
    this.flights.push(f2);
    this.flights.push(f3);
    this.flights.push(f4);
    this.flights.push(f5);
    this.flights.push(f6);
  }

  makeSeats() {
    return this.mockedSeats();
  }

  mockedSeats() {

    const seats = new Array<Seat>();

    const s1 = new Seat('F', 'A', 1);
    const s2 = new Seat('F', 'B', 1);
    const s3 = new Seat('F', 'C', 1);
    const s4 = new Seat('F', 'D', 1);
    const s5 = new Seat('F', 'E', 1);
    const s6 = new Seat('F', 'F', 1);
    const s7 = new Seat('F', 'A', 2);
    const s8 = new Seat('F', 'B', 2);
    const s9 = new Seat('F', 'C', 2);
    const s10 = new Seat('F', 'D', 2);
    const s11 = new Seat('F', 'E', 2);
    const s12 = new Seat('F', 'F', 2);
    const s13 = new Seat('E', 'A', 1);
    const s14 = new Seat('E', 'B', 1);
    const s15 = new Seat('E', 'C', 1);
    const s16 = new Seat('B', 'A', 1);
    const s17 = new Seat('B', 'B', 1);
    const s18 = new Seat('B', 'C', 1);
    const s19 = new Seat('B', 'D', 1);

    seats.push(s1);
    seats.push(s2);
    seats.push(s3);
    seats.push(s4);
    seats.push(s5);
    seats.push(s6);
    seats.push(s7);
    seats.push(s8);
    seats.push(s9);
    seats.push(s10);
    seats.push(s11);
    seats.push(s12);
    seats.push(s13);
    seats.push(s14);
    seats.push(s15);
    seats.push(s16);
    seats.push(s17);
    seats.push(s18);
    seats.push(s19);

    return seats;
  }
}
