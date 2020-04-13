import { User } from './user';
import { Flight } from './flight';
import { Car } from './car';

export class RegisteredUser extends User {
    friendsList: Array<RegisteredUser>;
    flights: Array<Flight>;
    rentedCars: Array<Car>;

    constructor(fname: string, lname: string, pasw: string, email: string, city: string, phone: string) {
        super(fname, lname, pasw, email, city, phone);
        this.friendsList = new Array<RegisteredUser>();
        this.rentedCars = new Array<Car>();
        this.flights = new Array<Flight>();
    }
}
