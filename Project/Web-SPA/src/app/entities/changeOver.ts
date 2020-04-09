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
        this.stopTime = (Math.abs(Number(this.departureTime.split(':')[0]) - Number(this.arrivalTime.split(':')[0]))).toString() + 'h' +
                        (Math.abs(Number(this.departureTime.split(':')[1]) - Number(this.arrivalTime.split(':')[1]))).toString() + 'min';
    }
}
