import { Injectable } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { Flight } from 'src/app/entities/flight';
import { AirlineService } from './airline.service';
import { SpecialOffer } from 'src/app/entities/special-offer';
import { timer } from 'rxjs';
import { Time } from '@angular/common';
import { Destination } from 'src/app/entities/destination';
import { ChangeOver } from 'src/app/entities/changeOver';
import { Seat } from 'src/app/entities/seat';

@Injectable({
  providedIn: 'root'
})
export class SpecialOfferService {

  specialOffers: Array<SpecialOffer>;
  constructor(private airlines: AirlineService) {
    this.specialOffers = new Array<SpecialOffer>();
    this.mockedSpecialOffers();
  }

  addSpecialOfferToAirline(flight: Flight, newPrice: number) {
    alert('adding spec offer. Not implemented');
  }

  getSpecialOffersOfSpecificAirline(airlineId: number) {
    let specOffers = new Array<SpecialOffer>();
    this.specialOffers.forEach(offer => {
      if (offer.airlineId == airlineId) {
        specOffers.push(offer);
      }
    });
    return specOffers;
  }


  mockedSpecialOffers() {
    const f1 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '5h 53min', 12,
    [new ChangeOver('11:20', '10:30', new Destination('', 'Paris', 'France', 'PAR'))], 300.00, '234T',
    new Destination('', 'Madrid', 'Spain', 'MAD'), new Destination('', 'Belgrade', 'Serbia', 'BG'), '06:20', '12:13',
    [new Seat(0, '33R')]);

    const s1 = new SpecialOffer([f1], 200.00, 'oneWay', 0);

    const f2 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '7h 0min', 12,
    [], 200.00, '234T',
    new Destination('', 'New York', 'USA', 'NY'), new Destination('', 'Novi Sad', 'Serbia', 'NS'), '15:15', '22:15',
    [new Seat(0, '33R')]);

    const f3 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '10h 0min', 12,
    [new ChangeOver('10:00', '09:00', new Destination('', 'Paris', 'France', 'PAR')),
    new ChangeOver('12:10', '11:20', new Destination('', 'London', 'England', 'LON'))], 200.00, '234T',
    new Destination('', 'Novi Sad', 'Serbia', 'NS'), new Destination('', 'New York', 'USA', 'NY'), '06:00', '16:00',
    [new Seat(0, '33R')]);

    // AKO JE ROUND TRIP MORA IMATI DVA LETA, NE JEDAN
    const s2 = new SpecialOffer([f3, f2], 400.00, 'roundTrip', 0);

    this.specialOffers.push(s1);
    console.log('SPECIAL OFFERS LENGTH:' + this.specialOffers.length);
    this.specialOffers.push(s2);
  }
}
