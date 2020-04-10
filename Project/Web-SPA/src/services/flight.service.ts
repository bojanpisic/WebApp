import { Injectable } from '@angular/core';
import { Flight } from 'src/app/entities/flight';
import { AirlineService } from './airline.service';
import { ChangeOver } from 'src/app/entities/changeOver';
import { Destination } from 'src/app/entities/destination';
import { Seat } from 'src/app/entities/seat';

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
    let f = new Array<Flight>();
    this.flights.forEach(a => {
      if (a.airlineId == airlineId) {
        f.push(a);
      }
    });
    return f;
  }


  mockedFlights() {
    const f1 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '03h 40min', 12,
    [new ChangeOver('11:20', '10:30',new Destination('', 'Paris', 'France', 'PAR'))], 300.00, '234T',
    new Destination('', 'Madrid', 'Spain', 'MAD'), new Destination('', 'Belgrade', 'Serbia', 'BG'), '06:20', '12:13',
    [new Seat(0, '33R')]);

    const f2 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '03h 40min', 12,
    [new ChangeOver('11:20', '10:30', new Destination('', 'Paris', 'France', 'PAR'))], 200.00, '234T',
    new Destination('', 'Novi Sad', 'Serbia', 'NS'), new Destination('', 'Barcelona', 'Spain', 'BAR'), '06:20', '12:13',
    [new Seat(0, '33R')]);

    this.flights.push(f1);
    this.flights.push(f2);
  }
}
