import { Destination } from './destination';
import { Time } from '@angular/common';

export class ChangeOver {
    private departureTime: string;
    private arrivalTime: string;
    private newDestination: Destination;
    public stopTime: string;

    constructor(depTime: string, arrTime: string, newDest: Destination) {
        this.departureTime = depTime;
        this.arrivalTime = arrTime;
        this.newDestination = newDest;

        const departureTimeInMinutes = Number(this.departureTime.split(':')[0]) * 60 + Number(this.departureTime.split(':')[1]);
        const arrivalTimeInMinutes = Number(this.arrivalTime.split(':')[0]) * 60 + Number(this.arrivalTime.split(':')[1]);

        this.stopTime = Math.floor((departureTimeInMinutes - arrivalTimeInMinutes) / 60) + 'h'
                      + Math.floor((departureTimeInMinutes - arrivalTimeInMinutes) % 60) + 'min';
    }
}
