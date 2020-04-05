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

    const a1 = new Airline('TurkishAirline', 'Istanbul, Turkey');
    a1.id = 0;
    a1.flightDestionations.push('Belgrade');
    a1.flightDestionations.push('Instanbul');
    a1.flightDestionations.push('Madrid');
    a1.flightDestionations.push('Berlin');
    a1.flightDestionations.push('London');
    a1.averageRating = 4.7;
    a1.promoDescription = 'Established back in 1933. Now we fly to more contries than any other airline.';


    const a2 = new Airline('QatarAirways', 'Doha, Qatar');
    a2.id = 1;

    a2.flightDestionations.push('Belgrade');
    a2.flightDestionations.push('Instanbul');
    a2.flightDestionations.push('Madrid');
    a2.flightDestionations.push('Berlin');
    a2.flightDestionations.push('London');
    a2.averageRating = 5.0;
    a2.promoDescription = 'World’s fastest-growing airline. We connect more than 160 destinations on the map every day.';

    const a3 = new Airline('AirSerbia', 'Belgrade, Serbia');
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
    a3.promoDescription = 'Founded in 1927. We offer a new concept of in-flight comfort across our growing network of lines.Maximum comfort while traveling, regardless of the class.';


    const a4 = new Airline('TurkishAirline', 'AIstanbul, Turkey');
    a1.flightDestionations.push('Belgrade');
    a1.flightDestionations.push('Instanbul');
    a1.flightDestionations.push('Madrid');
    a1.flightDestionations.push('Berlin');
    a1.flightDestionations.push('London');
    a1.averageRating = 4.7;
    a1.promoDescription = 'Established back in 1933. Now we fly to more contries than any other airline.';

    const a5 = new Airline('Air', 'Istanbul, Turkey');
    a1.flightDestionations.push('Belgrade');
    a1.flightDestionations.push('Instanbul');
    a1.flightDestionations.push('Madrid');
    a1.flightDestionations.push('Berlin');
    a1.flightDestionations.push('London');
    a1.averageRating = 4.7;
    a1.promoDescription = 'Established back in 1933. Now we fly to more contries than any other airline.';

    const a6 = new Airline('TurkishAirline', 'BIstanbul, Turkey');
    a1.flightDestionations.push('Belgrade');
    a1.flightDestionations.push('Instanbul');
    a1.flightDestionations.push('Madrid');
    a1.flightDestionations.push('Berlin');
    a1.flightDestionations.push('London');
    a1.averageRating = 4.7;
    a1.promoDescription = 'Established back in 1933. Now we fly to more contries than any other airline.';

    allAirlines.push(a1);
    allAirlines.push(a2);
    allAirlines.push(a3);
    allAirlines.push(a4);
    allAirlines.push(a5);
    allAirlines.push(a6);

    return allAirlines;
  }
}
