import { Seat } from './seat';

export class Ticket {
    flightIds: Array<number>;
    seat: Array<Seat>;
    price: number;

    constructor(flights: Array<number>, seats: Array<Seat>, price: number) {
        this.flightIds = flights;
        this.seat = seats;
        this.price = price;
    }

}
