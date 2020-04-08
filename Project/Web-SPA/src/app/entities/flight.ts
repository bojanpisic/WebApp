import { Time } from '@angular/common';
import { Destination } from './destination';

export class Flight {
    airlineId: number;
    flightNumber: string;
    takeOffDate: Date;
    landingDate: Date;
    tripTime: string;
    tripLength: number;
    changeOverLocations: Array<[string, Destination]>; // time, destination
    ticketPrice: number;
    from: Destination;
    to: Destination;
    takeOffTime: string;
    landingTime: string;

    constructor(airlineId: number, takeOffDate: Date, landingDate: Date, tripTime: string,
                tripLength: number, changeOverLocations: Array<[string, Destination]>,
                ticketPrice: number, flightNumber: string,
                from: Destination, to: Destination, takeofftime: string, landingtime: string) {
            this.airlineId = airlineId;
            this.takeOffDate = takeOffDate;
            this.landingDate = landingDate;
            this.tripTime = tripTime;
            this.tripLength = tripLength;
            this.changeOverLocations = changeOverLocations;
            this.ticketPrice = ticketPrice;
            this.flightNumber = flightNumber;
            this.from = from;
            this.to = to;
            this.landingTime = landingtime;
            this.takeOffTime = takeofftime;
    }
}
