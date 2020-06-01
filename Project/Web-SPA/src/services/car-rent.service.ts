import { Injectable } from '@angular/core';
import { RentACarService } from 'src/app/entities/rent-a-car-service';
import { Address } from 'src/app/entities/address';

@Injectable({
  providedIn: 'root'
})
export class CarRentService {

  rentACarServices: Array<RentACarService>;

  constructor() { 
    this.rentACarServices = new Array<RentACarService>();
    this.allMockedRentServices();
  }

  loadAllRentServices() {
    return this.allMockedRentServices();
  }

  getAdminsRac(adminId: number) {
    console.log(this.rentACarServices);
    return this.rentACarServices[0];
  }

  getAdminsRACId(adminId: number) {
    let retVal;
    this.rentACarServices.forEach(rac => {
      if (rac.adminId == adminId) {
        retVal = rac.id;
      }
    });
    return retVal;
  }

  allMockedRentServices() {

    const branch1 = new RentACarService('RentACarService01', new Address('Beograd', 'Srbija', 'BG', 20.30416545, 44.81833006));
    const branch2 = new RentACarService('RentACarService02', new Address('Beograd', 'Srbija', 'BG', 20.30416545, 44.81833006));
    const branch3 = new RentACarService('RentACarService03', new Address('Beograd', 'Srbija', 'BG', 20.30416545, 44.81833006));
    const branch4 = new RentACarService('RentACarService04', new Address('Beograd', 'Srbija', 'BG', 20.30416545, 44.81833006));
    const branch5 = new RentACarService('RentACarService05', new Address('Beograd', 'Srbija', 'BG', 20.30416545, 44.81833006));
    const branch6 = new RentACarService('RentACarService06', new Address('Beograd', 'Srbija', 'BG', 20.30416545, 44.81833006));


    const a1 = new RentACarService('Instanbul Rent Service', new Address('Istanbul', 'Turkey', 'IST', 32.974662768,  40.1077995688));
    a1.id = 0;
    a1.adminId = 33;
    a1.branches.push(branch1);
    a1.branches.push(branch2);
    a1.branches.push(branch3);
    a1.branches.push(branch4);
    a1.branches.push(branch5);

    a1.averageRating = 4.7;
    a1.about = 'Lorem ipsum dolor sit amet consectetur adipisicing elit. \
    Quidem, praesentium? Sunt ipsum deserunt similique unde quo beatae. \
    Expedita nemo veniam alias eum fuga ut quibusdam consequuntur labore, \
    dolorum odit aut blanditiis saepe sequi impedit error beatae quos natus \
    officia aliquam, aspernatur quod harum fugit molestias amet? Doloribus \
    tempore temporibus earum?';
    a1.promoDescription.push('Lorem ipsum, dolor sit amet consectetur adipisicing elit.\
                              Voluptas omnis accusamus, odio in blanditiis animi accusantium velit tempore,\
                              nam laborum quibusdam fugit aspernatur ullam nihil ipsam illo similique minima magni?');
    a1.promoDescription.push('Nihil iste aliquid officiis excepturi! Molestiae id commodi vero accusantium reiciendis a,\
                              magnam labore repellat impedit aliquid voluptatem molestias maxime quam autem, mollitia quas\
                              repellendus excepturi aut eveniet eum perspiciatis nihil dicta officia illo. Ducimus aliquid\
                              enim asperiores consequatur reprehenderit velit alias distinctio quia vitae, natus voluptatum\
                              libero at doloremque possimus unde consectetur, corporis nostrum voluptate in tempore quod ullam\
                              aliquam nulla? Nostrum impedit consequatur magni nulla quisquam temporibus laboriosam?');


    const a2 = new RentACarService('Doha Car Service', new Address('Doha', 'Qatar', 'DO',51.52245,25.27932 ));
    a2.id = 1;
    a2.branches.push(branch1);
    a2.branches.push(branch2);
    a2.branches.push(branch3);
    a2.branches.push(branch4);
    a2.branches.push(branch5);

    a2.averageRating = 4.7;
    a2.about = 'Lorem ipsum dolor sit amet consectetur adipisicing elit. \
    Quidem, praesentium? Sunt ipsum deserunt similique unde quo beatae. \
    Expedita nemo veniam alias eum fuga ut quibusdam consequuntur labore, \
    dolorum odit aut blanditiis saepe sequi impedit error beatae quos natus \
    officia aliquam, aspernatur quod harum fugit molestias amet? Doloribus \
    tempore temporibus earum?';
    a2.promoDescription.push('Lorem ipsum, dolor sit amet consectetur adipisicing elit.\
                              Voluptas omnis accusamus, odio in blanditiis animi accusantium velit tempore,\
                              nam laborum quibusdam fugit aspernatur ullam nihil ipsam illo similique minima magni?');
    a2.promoDescription.push('Nihil iste aliquid officiis excepturi! Molestiae id commodi vero accusantium reiciendis a,\
                              magnam labore repellat impedit aliquid voluptatem molestias maxime quam autem, mollitia quas\
                              repellendus excepturi aut eveniet eum perspiciatis nihil dicta officia illo. Ducimus aliquid\
                              enim asperiores consequatur reprehenderit velit alias distinctio quia vitae, natus voluptatum\
                              libero at doloremque possimus unde consectetur, corporis nostrum voluptate in tempore quod ullam\
                              aliquam nulla? Nostrum impedit consequatur magni nulla quisquam temporibus laboriosam?');

    const a3 = new RentACarService('Belgrade Rent Service', new Address('Beograd', 'Srbija', 'BG', 20.30416545, 44.81833006));
    a3.id = 2;
    a3.branches.push(branch1);
    a3.branches.push(branch2);
    a3.branches.push(branch3);
    a3.branches.push(branch4);
    a3.branches.push(branch5);

    a3.averageRating = 4.7;
    a3.about = 'Lorem ipsum dolor sit amet consectetur adipisicing elit. \
    Quidem, praesentium? Sunt ipsum deserunt similique unde quo beatae. \
    Expedita nemo veniam alias eum fuga ut quibusdam consequuntur labore, \
    dolorum odit aut blanditiis saepe sequi impedit error beatae quos natus \
    officia aliquam, aspernatur quod harum fugit molestias amet? Doloribus \
    tempore temporibus earum?';
    a3.promoDescription.push('Lorem ipsum, dolor sit amet consectetur adipisicing elit.\
                              Voluptas omnis accusamus, odio in blanditiis animi accusantium velit tempore,\
                              nam laborum quibusdam fugit aspernatur ullam nihil ipsam illo similique minima magni?');
    a3.promoDescription.push('Nihil iste aliquid officiis excepturi! Molestiae id commodi vero accusantium reiciendis a,\
                              magnam labore repellat impedit aliquid voluptatem molestias maxime quam autem, mollitia quas\
                              repellendus excepturi aut eveniet eum perspiciatis nihil dicta officia illo. Ducimus aliquid\
                              enim asperiores consequatur reprehenderit velit alias distinctio quia vitae, natus voluptatum\
                              libero at doloremque possimus unde consectetur, corporis nostrum voluptate in tempore quod ullam\
                              aliquam nulla? Nostrum impedit consequatur magni nulla quisquam temporibus laboriosam?');
    this.rentACarServices.push(a1);
    this.rentACarServices.push(a2);
    this.rentACarServices.push(a3);

    return this.rentACarServices;
  }
}
