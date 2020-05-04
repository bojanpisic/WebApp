export class Seat {
    class: string;
    column: string;
    row: number;
    available: boolean;
    reserved: boolean;

    constructor(classParam: string, columnParam: string, rowParam: number) {
        this.class = classParam;
        this.column = columnParam;
        this.row = rowParam;
        this.available = true;
        this.reserved = false;
    }
}
