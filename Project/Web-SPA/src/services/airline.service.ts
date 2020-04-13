import { Injectable } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { Destination } from 'src/app/entities/destination';
import { Flight } from 'src/app/entities/flight';
import { TripId } from 'src/app/entities/trip-id';
import { Address } from 'src/app/entities/address';

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

  allMockedAirlines() {

    const a1 = new Airline('TurkishAirlines', new Address('Istanbul', 'Turkey', 'IST', 32.974662768,  40.1077995688));
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
    a1.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAANrZ0RokHLnFBCXpktNOn7tHV7DFJu8W71TSSmVxilyXczl4Q7lQeI8Ho_9h0kQQ3fkasuGLi9x6bUbbP59VkLR7qcxRuuETaRpAVZoctDTJJVRQk9-2HYoMslWeOPU0KEhBWObed7DCtHeoeMJ2bR8q3GhQKngWJ9c2k8kiKwp5ZN4pAWMbdDw&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=42577',
      new Address('Belgrade', 'Serbia', 'BG', 0, 0)));
    // tslint:disable-next-line:max-line-length
    a1.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAA5lfOUh29fXPXOppkClh_zzT9DdVg3nobFIh6mPUYLAB1mpQNS1SdR4Mb8fG0fHuZfb9niTcMPeKQOLvgAyRWNkSvLGx3hR0po9NwG6s9YBi5UjCELIlfYrrN10YigzJVEhBWz-pc26INInazmrx4bUeqGhROU7BSle7lJDMT9wjjRBA2p-9ncA&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=72629', 
    new Address('Instanbul', 'Turkey', 'IST', 0, 0)));
    // tslint:disable-next-line:max-line-length
    a1.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAA2P3mLvl5uzffhueijdHj3omyLUfN-qf0YG-mO4WeDwrnX4QrkUIFvEQEPw7It8fAhSwpdxIQltnVpBeDbLahszSj6WwAKVoqHaWbVsb-LzYaWmZIt3ulCkgcyiuiBDQGEhBBhP_nH7ecCA-Nq6f0hgpsGhTR3VE2LWER4--5bCjMvwMLTjEmbw&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=54919', 
    new Address('Berlin', 'Germany','BER',0,0)));
    // tslint:disable-next-line:max-line-length
    a1.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAADRGe0kwEohIRG9H_k87IbRabfMZ1OIBDsXznqMcRKASkpoDrffk_296wya4YsOHZYDmQfCHM_sosAYrgVKA1oYXLlYMrjJSx7hMwguj2MCpk5HQXgllqZEqox1_oo4CAEhBideOSt0ST72C6_8MLSId3GhQxbjakpYbP-aEkGRme0sgkpzcgxA&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=128963', 
    new Address('London', 'UK', 'LON',0,0)));


    const a2 = new Airline('QatarAirways', new Address('Doha', 'Qatar', 'DO',51.52245,25.27932 ));
    a2.id = 1;
    a2.adminid = -1;
    // tslint:disable-next-line:max-line-length
    a2.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAANrZ0RokHLnFBCXpktNOn7tHV7DFJu8W71TSSmVxilyXczl4Q7lQeI8Ho_9h0kQQ3fkasuGLi9x6bUbbP59VkLR7qcxRuuETaRpAVZoctDTJJVRQk9-2HYoMslWeOPU0KEhBWObed7DCtHeoeMJ2bR8q3GhQKngWJ9c2k8kiKwp5ZN4pAWMbdDw&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=42577',
      new Address('Belgrade', 'Serbia', 'BG', 0, 0)));
    // tslint:disable-next-line:max-line-length
    a2.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAA5lfOUh29fXPXOppkClh_zzT9DdVg3nobFIh6mPUYLAB1mpQNS1SdR4Mb8fG0fHuZfb9niTcMPeKQOLvgAyRWNkSvLGx3hR0po9NwG6s9YBi5UjCELIlfYrrN10YigzJVEhBWz-pc26INInazmrx4bUeqGhROU7BSle7lJDMT9wjjRBA2p-9ncA&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=72629', 
    new Address('Instanbul', 'Turkey', 'IST', 0, 0)));
    // tslint:disable-next-line:max-line-length
    a2.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAA2P3mLvl5uzffhueijdHj3omyLUfN-qf0YG-mO4WeDwrnX4QrkUIFvEQEPw7It8fAhSwpdxIQltnVpBeDbLahszSj6WwAKVoqHaWbVsb-LzYaWmZIt3ulCkgcyiuiBDQGEhBBhP_nH7ecCA-Nq6f0hgpsGhTR3VE2LWER4--5bCjMvwMLTjEmbw&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=54919', 
    new Address('Berlin', 'Germany','BER',0,0)));
    // tslint:disable-next-line:max-line-length
    a2.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAADRGe0kwEohIRG9H_k87IbRabfMZ1OIBDsXznqMcRKASkpoDrffk_296wya4YsOHZYDmQfCHM_sosAYrgVKA1oYXLlYMrjJSx7hMwguj2MCpk5HQXgllqZEqox1_oo4CAEhBideOSt0ST72C6_8MLSId3GhQxbjakpYbP-aEkGRme0sgkpzcgxA&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=128963', 
    new Address('London', 'UK', 'LON',0,0)));
    a2.averageRating = 5.0;

    const a3 = new Airline('AirSerbia', new Address('Beograd', 'Srbija', 'BG', 20.30416545, 44.81833006));
    a3.id = 2;
    a3.adminid = 0;
    a3.averageRating = 4.3;
    // tslint:disable-next-line:max-line-length
    a3.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAANrZ0RokHLnFBCXpktNOn7tHV7DFJu8W71TSSmVxilyXczl4Q7lQeI8Ho_9h0kQQ3fkasuGLi9x6bUbbP59VkLR7qcxRuuETaRpAVZoctDTJJVRQk9-2HYoMslWeOPU0KEhBWObed7DCtHeoeMJ2bR8q3GhQKngWJ9c2k8kiKwp5ZN4pAWMbdDw&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=42577',
      new Address('Belgrade', 'Serbia', 'BG', 0, 0)));
    // tslint:disable-next-line:max-line-length
    a3.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAA5lfOUh29fXPXOppkClh_zzT9DdVg3nobFIh6mPUYLAB1mpQNS1SdR4Mb8fG0fHuZfb9niTcMPeKQOLvgAyRWNkSvLGx3hR0po9NwG6s9YBi5UjCELIlfYrrN10YigzJVEhBWz-pc26INInazmrx4bUeqGhROU7BSle7lJDMT9wjjRBA2p-9ncA&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=72629', 
    new Address('Instanbul', 'Turkey', 'IST', 0, 0)));
    // tslint:disable-next-line:max-line-length
    a3.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAA2P3mLvl5uzffhueijdHj3omyLUfN-qf0YG-mO4WeDwrnX4QrkUIFvEQEPw7It8fAhSwpdxIQltnVpBeDbLahszSj6WwAKVoqHaWbVsb-LzYaWmZIt3ulCkgcyiuiBDQGEhBBhP_nH7ecCA-Nq6f0hgpsGhTR3VE2LWER4--5bCjMvwMLTjEmbw&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=54919', 
    new Address('Berlin', 'Germany','BER',0,0)));
    // tslint:disable-next-line:max-line-length
    a3.flightDestionations.push(new Destination('https://maps.googleapis.com/maps/api/place/js/PhotoService.GetPhoto?1sCmRaAAAADRGe0kwEohIRG9H_k87IbRabfMZ1OIBDsXznqMcRKASkpoDrffk_296wya4YsOHZYDmQfCHM_sosAYrgVKA1oYXLlYMrjJSx7hMwguj2MCpk5HQXgllqZEqox1_oo4CAEhBideOSt0ST72C6_8MLSId3GhQxbjakpYbP-aEkGRme0sgkpzcgxA&3u165&4u112&5m1&2e1&callback=none&key=AIzaSyC0UzE_hJZ7OZahdEBDwBk0u4agqCQOsXE&token=128963', 
    new Address('London', 'UK', 'LON',0,0)));
    // tslint:disable-next-line: max-line-length
    a3.promoDescription.push('Founded in 1927. We offer a new concept of in-flight comfort across our growing network of lines.Maximum comfort while traveling, regardless of the class.');


    this.airlines.push(a1);
    this.airlines.push(a2);
    this.airlines.push(a3);
  }
}
