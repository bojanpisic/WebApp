import { Injectable } from '@angular/core';
import { RentACarService } from 'src/app/entities/rent-a-car-service';

@Injectable({
  providedIn: 'root'
})
export class CarRentService {

  constructor() { }
  
  loadAllRentServices() {
    return this.allMockedRentServices();
  }

  allMockedRentServices() {

    let allServices = new Array<RentACarService>();

    const a1 = new RentACarService('Rent1', 'Istanbul, Turkey');
    a1.branches.push('Belgrade');
    a1.branches.push('Instanbul');
    a1.branches.push('Madrid');
    a1.branches.push('Berlin');
    a1.branches.push('London');
    a1.averageRating = 4.7;
    a1.promoDescription = 'Established back in 1933. Now we fly to more contries than any other airline.';


    const a2 = new RentACarService('Rent2', 'Doha, Qatar');
    a2.branches.push('Belgrade');
    a2.branches.push('Instanbul');
    a2.branches.push('Madrid');
    a2.branches.push('Berlin');
    a2.branches.push('London');
    a2.averageRating = 5.0;
    a2.promoDescription = 'World’s fastest-growing airline. We connect more than 160 destinations on the map every day.';

    const a3 = new RentACarService('Rent3', 'Belgrade, Serbia');
    a3.branches.push('Belgrade');
    a3.branches.push('Instanbul');
    a3.branches.push('Madrid');
    a3.branches.push('Berlin');
    a3.branches.push('London');
    a3.branches.push('Madrid');
    a3.branches.push('Berlin');
    a3.branches.push('London');
    a3.averageRating = 4.3;
    // tslint:disable-next-line: max-line-length
    a3.promoDescription = 'Founded in 1927. We offer a new concept of in-flight comfort across our growing network of lines.Maximum comfort while traveling, regardless of the class.';

    allServices.push(a1);
    allServices.push(a2);
    allServices.push(a3);

    return allServices;
  }
}
