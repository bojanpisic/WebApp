import { Flight } from './flight';
import { Seat } from './seat';
import { SeatsForFlight } from './seats-for-flight';
import { PassengersForFlight } from './passengers-for-flight';

export class TripReservation {
    flights: Array<Flight>;
    seats: Array<SeatsForFlight>;
    passengers: Array<PassengersForFlight>;

    constructor() {
        this.flights = new Array<Flight>();
        this.seats = new Array<SeatsForFlight>();
        this.passengers = new Array<PassengersForFlight>();
    }
}
