import { Flight } from './flight';
import { Seat } from './seat';
import { Passenger } from './passenger';

export class PassengersForFlight {
    public flight: Flight; // napisano skraceno da bi izbegli predugacak url
    public passengers: Array<Passenger>;
    constructor(f: Flight, p: Array<Passenger>) {
      this.flight = f;
      this.passengers = p;
    }
}
