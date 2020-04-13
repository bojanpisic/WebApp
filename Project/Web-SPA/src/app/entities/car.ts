export class Car {

    id: number;
    idOfService: number; // id glavnog servisa
    idOfAffiliate: number; // id filijale u kojoj je, ako je u glavnoj onda je 0
    brand: string;
    model: string;
    year: number;
    type: string; // small, medium, large, suv, van, luxury, convertible
    numberOfSeats: number;
    averageRate: number;
    pricePerDay: number;
    automatic: true; // false ako je manual gearbox

    constructor(id: number, idOfService: number, idOfAffiliate: number, brand: string, model: string, year: number, type: string,
                numberOfSeats: number, averageRate: number, pricePerDay: number) {
        this.id = id;
        this.idOfService = idOfService;
        this.idOfAffiliate = idOfAffiliate;
        this.brand = brand;
        this.model = model;
        this.year = year;
        this.type = type;
        this.numberOfSeats = numberOfSeats;
        this.averageRate = averageRate;
        this.pricePerDay = pricePerDay;
    }
}
