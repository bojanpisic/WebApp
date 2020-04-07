import { Injectable } from '@angular/core';
import { Airline } from 'src/app/entities/airline';

@Injectable({
  providedIn: 'root'
})
export class AirlineService {

  constructor() { }

  loadAllAirlines() {
    return this.allMockedAirlines();
  }

  allMockedAirlines() {

    let allAirlines = new Array<Airline>();

    const a1 = new Airline('TurkishAirline', ['', 40.1077995688, 32.974662768]);
    a1.id = 0;
    a1.adminid = -1;

    a1.flightDestionations.push('Belgrade');
    a1.flightDestionations.push('Instanbul');
    a1.flightDestionations.push('Madrid');
    a1.flightDestionations.push('Berlin');
    a1.flightDestionations.push('London');
    a1.averageRating = 4.7;
    a1.promoDescription.push('Turkish airlines’ history as pioneer of the sky began in the year 1933.\
    We started out as a small team and with perseverance and growing passion we branched out and\
    become one huge family.It is with great pride and joy that we were the first to fly the sky for our country.');
    a1.promoDescription.push('Keeping up to date with technology is an essential component of our innovation aims and in maintaining that\
    we have the youngest and most modern fleet in Europe. Our fleet had flourished thanks to our high-tech,\
    fuel-efficient and environmentally conscious aircraft purchases that provide a high level of comfort. ');


    const a2 = new Airline('QatarAirways', ['', 25.27932, 51.52245]);
    a2.id = 1;
    a2.adminid = -1;
    a2.flightDestionations.push('Belgrade');
    a2.flightDestionations.push('Instanbul');
    a2.flightDestionations.push('Madrid');
    a2.flightDestionations.push('Berlin');
    a2.flightDestionations.push('London');
    a2.averageRating = 5.0;
    // a2.promoDescription = 'Lorem ipsum dolor sit amet, consectetur adipisicing elit.\
    //                        Corporis beatae adipisci consectetur ad rerum aspernatur, dicta omnis at ipsa explicabo\
    //                        aperiam saepe optio, possimus nostrum aliquid commodi ex nesciunt non consequatur in\
    //                        delectus tempore maxime? Nihil aliquam nobis nesciunt earum nulla. Voluptate, magni quas.\
    //                        Cum molestiae voluptatem ea placeat quod?';

    const a3 = new Airline('AirSerbia', ['', 44.81833006, 20.30416545]);
    a3.id = 2;
    a3.adminid = 0;
    a3.flightDestionations.push('Belgrade');
    a3.flightDestionations.push('Instanbul');
    a3.flightDestionations.push('Madrid');
    a3.flightDestionations.push('Berlin');
    a3.flightDestionations.push('London');
    a3.flightDestionations.push('Madrid');
    a3.flightDestionations.push('Berlin');
    a3.flightDestionations.push('London');
    a3.averageRating = 4.3;
    // tslint:disable-next-line: max-line-length
    a3.promoDescription.push('Founded in 1927. We offer a new concept of in-flight comfort across our growing network of lines.Maximum comfort while traveling, regardless of the class.');


    const a4 = new Airline('TurkishAirline', ['', 44.787197, 20.457273]);
    a4.flightDestionations.push('Belgrade');
    a4.flightDestionations.push('Instanbul');
    a4.flightDestionations.push('Madrid');
    a4.flightDestionations.push('Berlin');
    a4.flightDestionations.push('London');
    a4.averageRating = 4.7;
    // a4.promoDescription = 'Established back in 1933. Now we fly to more contries than any other airline.';


    const a5 = new Airline('Air', ['', 44.787197, 20.457273]);
    a5.flightDestionations.push('Belgrade');
    a5.flightDestionations.push('Instanbul');
    a5.flightDestionations.push('Madrid');
    a5.flightDestionations.push('Berlin');
    a5.flightDestionations.push('London');
    a5.averageRating = 4.7;
    // a5.promoDescription = 'Established back in 1933. Now we fly to more contries than any other airline.';

    const a6 = new Airline('TurkishAirline', ['', 44.787197, 20.457273]);
    a6.flightDestionations.push('Belgrade');
    a6.flightDestionations.push('Instanbul');
    a6.flightDestionations.push('Madrid');
    a6.flightDestionations.push('Berlin');
    a6.flightDestionations.push('London');
    a6.averageRating = 4.7;
    // a6.promoDescription = 'Established back in 1933. Now we fly to more contries than any other airline.';

    allAirlines.push(a1);
    allAirlines.push(a2);
    allAirlines.push(a3);
    allAirlines.push(a4);
    allAirlines.push(a5);
    allAirlines.push(a6);

    return allAirlines;
  }
}
