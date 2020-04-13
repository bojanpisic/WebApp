import { Destination } from './destination';
import { Address } from './address';

export class RentACarService {

    id: number;
    adminId: number;
    name: string;
    address: Address;
    promoDescription: string;
    branches: Array<string>;
    averageRating: number;
    rates: Array<number>;

    constructor(name: string, address: Address) {
        this.name = name;
        this.address = address;
        this.promoDescription = '';
        this.branches = new Array<string>();
        this.averageRating = 0.0;
        this.rates = new Array<number>();
    }
}
