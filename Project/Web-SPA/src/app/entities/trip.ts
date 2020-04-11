import { Seat } from './seat';
import { Flight } from './flight';
import { TripId } from './trip-id';
import { ArrayType } from '@angular/compiler';

export class Trip {
    // tripId: TripId; UMESTO FLIGHTS
    flights: Array<Flight>;
    tripType: string;
    minumumPrice: number;

    constructor(flights: Array<Flight>, tripType: string, minimumPrice: number) {
        this.flights = flights;
        this.tripType = tripType;
        this.minumumPrice = minimumPrice;
    }

}
