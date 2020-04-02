export class RentACarService {

    id: number;
    name: string;
    address: string;
    promoDescription: string;
    branches: Array<string>;
    averageRating: number;
    rates: Array<number>;

    constructor(name: string, address: string) {
        this.name = name;
        this.address = address;
        this.promoDescription = '';
        this.branches = new Array<string>();
        this.averageRating = 0.0;
        this.rates = new Array<number>();
    }
}
