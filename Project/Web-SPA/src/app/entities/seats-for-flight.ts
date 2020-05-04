import { Flight } from './flight';
import { Seat } from './seat';

export class SeatsForFlight {
    public flight: Flight; // napisano skraceno da bi izbegli predugacak url
    public seats: Array<Seat>;
    constructor() {
      this.seats = new Array<Seat>();
    }
}
