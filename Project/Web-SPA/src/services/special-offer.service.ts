import { Injectable } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { Flight } from 'src/app/entities/flight';
import { AirlineService } from './airline.service';
import { SpecialOffer } from 'src/app/entities/special-offer';
import { timer } from 'rxjs';
import { Time } from '@angular/common';
import { Destination } from 'src/app/entities/destination';

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
      if (offer.flight.airlineId == airlineId) {
        specOffers.push(offer);
      }
    });

    return specOffers;
  }
  
  mockedSpecialOffers() {
    const f1 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '03h 40min', 12,
    [['10:45', new Destination('', 'Paris', 'France', 'PAR')], ['10:45', new Destination('', 'Barcelona', 'Spain', 'BAR')]], 300.00, '234T',
    new Destination('', 'Belgrade', 'Serbia', 'BG'), new Destination('', 'Madrid', 'Spain', 'MAD'), '06:20', '12:13');

    const s1 = new SpecialOffer(f1, '12A', 200.00);

    const s2 = new SpecialOffer(
      new Flight( 0, new Date(Date.now()), new Date(Date.now()), '06h 07min', 12, 
      [['10:45', new Destination('', 'Paris', 'France', 'PAR')], ['10:45', new Destination('', 'Barcelona', 'Spain', 'BAR')]], 4300.00, '899R',
      new Destination('', 'Instanbul', 'Turkey', 'IST'), new Destination('', 'Berlin', 'Germany', 'BER'), '06:20', '12:13'),
      '5E', 370.00);

    this.specialOffers.push(s1);
    this.specialOffers.push(s2);
  }
}
