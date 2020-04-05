import { Time } from '@angular/common';

export class Flight {
    takeOffTime: Date;
    landingTime: Date;
    tripTime: Time;
    tripLength: number;
    changeOverLocations: Array<string>;
    numOfChangeOvers: number;
    ticketPrice: number;

    constructor(takeOffTime: Date, landingTime: Date, tripTime: Time,
                tripLength: number, changeOverLocations: Array<string>, numOfChangeOvers: number, ticketPrice: number) {

            this.takeOffTime = takeOffTime;
            this.landingTime = landingTime;
            this.tripTime = tripTime;
            this.tripLength = tripLength;
            this.changeOverLocations = changeOverLocations;
            this.numOfChangeOvers = numOfChangeOvers;
            this.ticketPrice = ticketPrice;
    }
}
