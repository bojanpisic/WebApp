export class Destination {
    imageUrl: string;
    city: string;
    state: string;
    shortName: string;

    constructor(imgUrl: string, city: string, state: string, shortname: string) {
        this.imageUrl = imgUrl;
        this.city = city;
        this.state = state;
        this.shortName = shortname;
    }
}
