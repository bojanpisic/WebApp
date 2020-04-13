import { Injectable } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { Destination } from 'src/app/entities/destination';
import { Flight } from 'src/app/entities/flight';
import { TripId } from 'src/app/entities/trip-id';

@Injectable({
  providedIn: 'root'
})
export class AirlineService {

  airlines: Array<Airline>;
  flightsForTrip: Array<Flight>;
  constructor() {
    this.airlines = new Array<Airline>();
    this.flightsForTrip = new Array<Flight>();
    this.allMockedAirlines();
   }

  loadAllAirlines() {
    const retValue = new Array<Airline>();
    for (const item of this.airlines) {
      retValue.push(item);
    }
    return retValue;
  }

  getAirline(id: number) {
    let air: Airline;
    this.airlines.forEach(airline => {
      if (airline.id == id) {
        air = airline;
      }
    });
    return air;
  }

  // getFlightsForTrip(tripId: TripId) {
  //   const flights = new Array<Flight>();
  //   let i = 0;
  //   for (const item of this.airlines) {
  //     if (item.id === tripId.airlineIds[i]) {
  //       for (const flight of item.flights) {
  //         if (flight.flightNumber === tripId.flightsIds[i]) {
  //           flights.push(flight);
  //         }
  //       }
  //       i += 1;
  //     }
  //   }
  // }

  allMockedAirlines() {

    const a1 = new Airline('TurkishAirlines', ['', 40.1077995688, 32.974662768]);
    a1.id = 0;
    a1.logoUrl = '../../../../assets/turkish_airlines_logo.png';
    a1.adminid = -1;
    a1.averageRating = 4.7;
    a1.promoDescription.push('Turkish airlines’ history as pioneer of the sky began in the year 1933.\
    We started out as a small team and with perseverance and growing passion we branched out and\
    become one huge family.It is with great pride and joy that we were the first to fly the sky for our country.');
    a1.promoDescription.push('Keeping up to date with technology is an essential component of our innovation aims and in maintaining that\
    we have the youngest and most modern fleet in Europe. Our fleet had flourished thanks to our high-tech,\
    fuel-efficient and environmentally conscious aircraft purchases that provide a high level of comfort. ');
    // tslint:disable-next-line:max-line-length
    a1.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAAJjefkEa6Yz0ln6aw3QMbcoSq5h6A0bqrZWYulINfBQ6moJ9sNHzaAuWayuSVY5dKSpx6AMfqlycO_idIppJoF1HF-mIziwcaOzqZXaJy8ypbpkNTiAE1-Unvc5nmkmVTEhDbjVEtzS2PGdBnuOSUI1jGGhSWE0vCQdKt4lUdwXnkDqE5c4mubA&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=82881', 'Belgrade', 'Serbia', 'BG'));
    // tslint:disable-next-line:max-line-length
    a1.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAAvRCvcmBB933jEgkxe01viVqffaJ93xSQK7JXq1pLKC5S1UCqvU6LK_wJ-FTEB_kMWUHLZ1xfiw-2mb6JCecqQxlZQLIvZ7xvjSV5BrVSaVEmkTlhEJpQOPYaOgsNK8SfEhAnQOE5SkEgZS-C1rZv--JoGhT3kLbfH2bNMogrQt7G5jaXJ1Vz-A&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=47718', 'Instanbul', 'Turkey', 'IST'));
    // tslint:disable-next-line:max-line-length
    a1.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAAANGchNCx123LryABu6uYusFbnTk9kS9V3vAdPLMnvxLVpQPLH9Drzj6ecMKCPREzATK8ty2cMQSLeedWyD3XxY9gRTEMOX_Z6XTVfw4ifU3Dy1Ar6PUbCWzHRH172w2HEhCErSPn9v8_Ac42PyoC7MoTGhQ9ff3UvtO6WGw2ueLLM9xXpTtNDA&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=26411', 'Berlin', 'Germany','BER'));
    // tslint:disable-next-line:max-line-length
    a1.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAAHGuMOfwCKPSR7dMDgoQbKF2yQNn6eEie0OjdbkMEG4GdZ6D7xCJWcxaXzA9kjqpXY9_UKXSGSU8xeTBB0VE3Ata_y_kyOBfABXmeUkYqopBYvb3Cg4o5SHHtdb9G5susEhCgq2TRTjFpYSLNl8mFPVuIGhQkWT3TUKqi9kBcJ3UNoD2wMVtM5w&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=47953', 'London', 'UK', 'LON'));

    const a2 = new Airline('QatarAirways', ['Doha, Qatar', 25.27932, 51.52245]);
    a2.id = 1;
    a2.adminid = -1;
    a2.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAAJjefkEa6Yz0ln6aw3QMbcoSq5h6A0bqrZWYulINfBQ6moJ9sNHzaAuWayuSVY5dKSpx6AMfqlycO_idIppJoF1HF-mIziwcaOzqZXaJy8ypbpkNTiAE1-Unvc5nmkmVTEhDbjVEtzS2PGdBnuOSUI1jGGhSWE0vCQdKt4lUdwXnkDqE5c4mubA&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=82881', 'Belgrade', 'Serbia', 'BG'));
    a2.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAAvRCvcmBB933jEgkxe01viVqffaJ93xSQK7JXq1pLKC5S1UCqvU6LK_wJ-FTEB_kMWUHLZ1xfiw-2mb6JCecqQxlZQLIvZ7xvjSV5BrVSaVEmkTlhEJpQOPYaOgsNK8SfEhAnQOE5SkEgZS-C1rZv--JoGhT3kLbfH2bNMogrQt7G5jaXJ1Vz-A&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=47718', 'Instanbul', 'Turkey','IST'));
    a2.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAAANGchNCx123LryABu6uYusFbnTk9kS9V3vAdPLMnvxLVpQPLH9Drzj6ecMKCPREzATK8ty2cMQSLeedWyD3XxY9gRTEMOX_Z6XTVfw4ifU3Dy1Ar6PUbCWzHRH172w2HEhCErSPn9v8_Ac42PyoC7MoTGhQ9ff3UvtO6WGw2ueLLM9xXpTtNDA&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=26411', 'Berlin', 'Germany', 'BER'));
    a2.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAAHGuMOfwCKPSR7dMDgoQbKF2yQNn6eEie0OjdbkMEG4GdZ6D7xCJWcxaXzA9kjqpXY9_UKXSGSU8xeTBB0VE3Ata_y_kyOBfABXmeUkYqopBYvb3Cg4o5SHHtdb9G5susEhCgq2TRTjFpYSLNl8mFPVuIGhQkWT3TUKqi9kBcJ3UNoD2wMVtM5w&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=47953', 'London', 'UK', 'LON'));
    a2.averageRating = 5.0;
    // a2.promoDescription = 'Lorem ipsum dolor sit amet, consectetur adipisicing elit.\
    //                        Corporis beatae adipisci consectetur ad rerum aspernatur, dicta omnis at ipsa explicabo\
    //                        aperiam saepe optio, possimus nostrum aliquid commodi ex nesciunt non consequatur in\
    //                        delectus tempore maxime? Nihil aliquam nobis nesciunt earum nulla. Voluptate, magni quas.\
    //                        Cum molestiae voluptatem ea placeat quod?';

    const a3 = new Airline('AirSerbia', ['', 44.81833006, 20.30416545]);
    a3.id = 2;
    a3.adminid = 0;
    a3.averageRating = 4.3;
    a3.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAAJjefkEa6Yz0ln6aw3QMbcoSq5h6A0bqrZWYulINfBQ6moJ9sNHzaAuWayuSVY5dKSpx6AMfqlycO_idIppJoF1HF-mIziwcaOzqZXaJy8ypbpkNTiAE1-Unvc5nmkmVTEhDbjVEtzS2PGdBnuOSUI1jGGhSWE0vCQdKt4lUdwXnkDqE5c4mubA&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=82881', 'Belgrade', 'Serbia', 'BG'));
    a3.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAAvRCvcmBB933jEgkxe01viVqffaJ93xSQK7JXq1pLKC5S1UCqvU6LK_wJ-FTEB_kMWUHLZ1xfiw-2mb6JCecqQxlZQLIvZ7xvjSV5BrVSaVEmkTlhEJpQOPYaOgsNK8SfEhAnQOE5SkEgZS-C1rZv--JoGhT3kLbfH2bNMogrQt7G5jaXJ1Vz-A&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=47718', 'Instanbul', 'Turkey', 'IST'));
    a3.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAAANGchNCx123LryABu6uYusFbnTk9kS9V3vAdPLMnvxLVpQPLH9Drzj6ecMKCPREzATK8ty2cMQSLeedWyD3XxY9gRTEMOX_Z6XTVfw4ifU3Dy1Ar6PUbCWzHRH172w2HEhCErSPn9v8_Ac42PyoC7MoTGhQ9ff3UvtO6WGw2ueLLM9xXpTtNDA&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=26411', 'Berlin', 'Germany', 'BER'));
    a3.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAAHGuMOfwCKPSR7dMDgoQbKF2yQNn6eEie0OjdbkMEG4GdZ6D7xCJWcxaXzA9kjqpXY9_UKXSGSU8xeTBB0VE3Ata_y_kyOBfABXmeUkYqopBYvb3Cg4o5SHHtdb9G5susEhCgq2TRTjFpYSLNl8mFPVuIGhQkWT3TUKqi9kBcJ3UNoD2wMVtM5w&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=47953', 'London', 'UK', 'LON'));
    // tslint:disable-next-line: max-line-length
    a3.promoDescription.push('Founded in 1927. We offer a new concept of in-flight comfort across our growing network of lines.Maximum comfort while traveling, regardless of the class.');


    const a4 = new Airline('TurkishAirline', ['', 44.787197, 20.457273]);
    a4.averageRating = 4.7;
    // a4.promoDescription = 'Established back in 1933. Now we fly to more contries than any other airline.';


    const a5 = new Airline('Air', ['', 44.787197, 20.457273]);
    a5.averageRating = 4.7;
    // a5.promoDescription = 'Established back in 1933. Now we fly to more contries than any other airline.';

    const a6 = new Airline('TurkishAirline', ['', 44.787197, 20.457273]);
    a6.averageRating = 4.7;
    // a6.promoDescription = 'Established back in 1933. Now we fly to more contries than any other airline.';

    this.airlines.push(a1);
    this.airlines.push(a2);
    this.airlines.push(a3);
    this.airlines.push(a4);
    this.airlines.push(a5);
    this.airlines.push(a6);
  }
}
