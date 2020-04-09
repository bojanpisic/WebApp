export class Seat {
    flightId: number;
    seatNumber: string;

    constructor(flighId: number, seatNum: string) {
        this.flightId = flighId;
        this.seatNumber = seatNum;
    }
}
