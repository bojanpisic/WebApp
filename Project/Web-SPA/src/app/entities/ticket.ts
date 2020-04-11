import { Seat } from './seat';
import { Flight } from './flight';
import { Trip } from './trip';

export class Ticket {
    trip: Trip;
    seats: Array<Seat>;
    price: number;

    constructor(trip: Trip, seats: Array<Seat>, price: number) {
        this.trip = trip;
        this.seats = seats;
        this.price = price;
    }

}
