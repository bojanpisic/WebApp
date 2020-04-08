import { Flight } from './flight';

export class SpecialOffer {
    flight: Flight;
    seat: string;
    newPrice: number;

    constructor(flight: Flight, seat: string, newPrice: number) {
        this.flight = flight;
        this.seat = seat;
        this.newPrice = newPrice;
    }
}
