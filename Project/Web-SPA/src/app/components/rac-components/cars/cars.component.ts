import { Component, OnInit } from '@angular/core';
import { CarService } from 'src/services/car.service';
import { Car } from 'src/app/entities/car';
import { ActivatedRoute, Router } from '@angular/router';
import { RegisteredUser } from 'src/app/entities/registeredUser';
import { UserService } from 'src/services/user.service';
import { Location } from '@angular/common';
import { CarRentService } from 'src/services/car-rent.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cars',
  templateUrl: './cars.component.html',
  styleUrls: ['./cars.component.scss']
})
export class CarsComponent implements OnInit {

  userId: number;
  user: RegisteredUser;
  cars: Array<any>;

  url: any;
  urlParams = [];

  selectedCar = null;
  showModal = false;

  filter = false;

  constructor(private userService: UserService, private carService: CarRentService,
              private routes: ActivatedRoute, private location: Location,
              private router: Router,
              private toastr: ToastrService) {
    const array = routes.snapshot.queryParamMap.get('array');
    this.urlParams = JSON.parse(array);
    console.log(this.urlParams);

    routes.params.subscribe(param => {
      this.userId = param.id;
    });
    this.cars = new Array<any>();
   }

  ngOnInit(): void {
    let data;
    data = this.generateFilter();

    if (this.urlParams !== null) {
      this.url = {
        type: data.type,
        from: data.from,
        to: data.to,
        dep: data.dep,
        ret: data.ret,
        minPrice: data.minPrice,
        maxPrice: data.maxPrice,
        racs: data.racs,
        seatFrom: data.seatFrom,
        seatTo: data.seatTo
      };

      // this.cars = this.carService.getAllCars();

      const a = this.carService.test(this.url).subscribe(
        (res: any[]) => {
          if (res.length > 0) {
            res.forEach(el => {
              const r = {
                brand: el.brand,
                carId: el.carId,
                city: el.city,
                model: el.model,
                name: el.name,
                pricePerDay: el.pricePerDay,
                seatsNumber: el.seatsNumber,
                state: el.state,
                type: el.type,
                year: el.year
              };
              this.cars.push(r);
            });
          }
          console.log(res);
        },
        err => {
          console.log(err);
        }
      );
    }
  }

  onModal(value: boolean) {
    if (value) {
      const data = {
        from: this.url.from,
        to: this.url.to,
        dep: this.url.dep,
        ret: this.url.ret,
        carId: this.selectedCar.carId,
        userId: this.userId,
        totalPrice: this.selectedCar.totalPrice
      };
      alert(data.carId + " ma " + this.selectedCar);
      this.carService.reserveCar(data).subscribe(
        (res: any) => {
          this.toastr.success('Success!');
          this.router.navigate(['/' + this.userId + '/home']);
          this.showModal = false;
        },
        err => {
          // tslint:disable-next-line: triple-equals
          if (err.status == 400) {
            console.log(err);
            // this.toastr.error('Incorrect username or password.', 'Authentication failed.');
            this.toastr.error(err.statusText, 'Error!');
          } else {
            this.toastr.error(err.error.statusText, 'Error!');
          }
          this.showModal = false;
        }
      );
    } else {
      this.showModal = false;
    }
  }

  onBook(value: any) {
    const data = {
      carId: value,
      from: this.url.from,
      to: this.url.to,
      dep: this.url.dep,
      ret: this.url.ret,
    };

    const a = this.carService.getTotalPriceForResevation(data).subscribe(
        (res: any) => {
          this.selectedCar = {
            from: res.from,
            to: res.to,
            dep: res.dep,
            ret: res.ret,
            brand: res.brand,
            carId: res.carId,
            model: res.model,
            name: res.name, // company name
            totalPrice: res.totalPrice, // pricePerDay * broj dana
            seatsNumber: res.seatsNumber,
            type: res.type,
            year: res.year
          }
          this.showModal = true;
        },
        err => {
          console.log(err);
          this.toastr.error(err.error.statusText, 'Error!');
        }
      );
    


    // ******************************************* ODKOMENTARISI OVO GORE A ZAKOMENTASI OVO DOLE



    // this.selectedCar = {
    //   from: this.url.from,
    //   to: this.url.to,
    //   dep: this.url.dep,
    //   ret: this.url.ret,
    //   brand: 'Range Rover',
    //   carId: 1,
    //   city: 'Berlin',
    //   model: 'Evoque',
    //   name: 'Hertz',
    //   pricePerDay: 50,
    //   seatsNumber: 4,
    //   state: 'Germany',
    //   type: 'Luxury',
    //   year: 2020
    // };
    // this.showModal = true;
  }

  generateFilter() {
    if (this.urlParams === null && this.userId === undefined) {
      this.router.navigate(['']);
      return;
    }
    if (this.urlParams === null && this.userId !== undefined) {
      this.router.navigate(['/' + this.userId + '/home']);
      return;
    }
    if (this.urlParams !== null) {
      return {type: this.urlParams[0].type, from: this.urlParams[0].from, to: this.urlParams[0].to,
      dep: this.urlParams[0].dep, ret: this.urlParams[0].ret, minPrice: this.urlParams[0].minPrice,
      maxPrice: this.urlParams[0].maxPrice,
      racs: this.urlParams[0].racs, seatFrom: this.urlParams[0].seatFrom, seatTo: this.urlParams[0].seatTo};
    }
  }

  goBack() {
    this.location.back();
  }
  onApplyFilter() {

  }

  toggleFilter() {
    this.filter = !this.filter;
  }
}
