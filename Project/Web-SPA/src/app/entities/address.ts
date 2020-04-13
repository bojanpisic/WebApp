export class Address {
    city: string;
    state: string;
    lon: number;
    lat: number;
    shortName: string;

    constructor(city: string, state: string, shortname: string, lon: number, lat: number) {
        this.city = city;
        this.state = state;
        this.shortName = shortname;
        this.lat = lat;
        this.lon = lon;
    }
}
