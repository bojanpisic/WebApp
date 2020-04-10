import { Flight } from './flight';

export class SpecialOffer {
    flights: Array<Flight>;
    newPrice: number;
    tripType: string;
    airlineId: number;

    constructor(flight: Array<Flight>, newPrice: number, type: string, airlineId: number) {
        this.flights = flight;
        this.newPrice = newPrice;
        this.tripType = type;
        this.airlineId = airlineId;
    }
}
