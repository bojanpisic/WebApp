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
import { FlightService } from './flight.service';

@Injectable({
  providedIn: 'root'
})
export class SpecialOfferService {

  specialOffers: Array<SpecialOffer>;
  constructor(private airlines: AirlineService, private flightService: FlightService) {
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

    const flights = this.flightService.getAllFlights();

    // AKO JE ROUND TRIP MORA IMATI DVA LETA, NE JEDAN
    const s1 = new SpecialOffer([flights[0]], 200.00, 'oneWay', 0);
    const s2 = new SpecialOffer([flights[2], flights[3]], 1000.00, 'roundTrip', 0);
    const s3 = new SpecialOffer([flights[4], flights[0], flights[1]], 300.00, 'multicity', 0);
    const s4 = new SpecialOffer([flights[4]], 130.00, 'oneWay', 0);
    const s5 = new SpecialOffer([flights[5]], 100.00, 'oneWay', 0);
    const s6 = new SpecialOffer([flights[1]], 150.00, 'oneWay', 0);

    this.specialOffers.push(s1);
    this.specialOffers.push(s2);
    this.specialOffers.push(s3);
    this.specialOffers.push(s4);
    this.specialOffers.push(s5);
    this.specialOffers.push(s6);
  }
}
