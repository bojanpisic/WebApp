import { Flight } from './flight';

export class Airline {

    id: number;
    name: string;
    address: string;
    promoDescription: string;
    flightDestionations: Array<string>;
    flights: Array<Flight>;
    averageRating: number;
    rates: Array<number>;

    constructor(name: string, address: string) {
        this.name = name;
        this.address = address;
        this.promoDescription = '';
        this.flightDestionations = new Array<string>();
        this.flights = new Array<Flight>();
        this.averageRating = 0.0;
        this.rates = new Array<number>();
    }

    rateAriline(rate: number) {
        this.rates.push(rate);
        let sum = 0.00;

        this.rates.forEach(element => {
            sum += element;
        });

        this.averageRating = sum / this.rates.length;
    }
}
