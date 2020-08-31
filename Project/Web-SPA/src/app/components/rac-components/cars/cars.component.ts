import { Component, OnInit } from '@angular/core';
import { Car } from 'src/app/entities/car';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { RegisteredUser } from 'src/app/entities/registeredUser';
import { UserService } from 'src/services/user.service';
import { Location } from '@angular/common';
import { CarRentService } from 'src/services/car-rent.service';
import { ToastrService } from 'ngx-toastr';
import { FlightPlusCarService } from 'src/services/flight-plus-car.service';

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

  selectedOffer = null;
  showModal = false;

  filter = false;

  mySubscription;
  flights;
  showModalError = false;

  constructor(private userService: UserService, private carService: CarRentService,
              private routes: ActivatedRoute, private location: Location,
              private router: Router,
              private toastr: ToastrService,
              private flightPlusCar: FlightPlusCarService) {
    const array = routes.snapshot.queryParamMap.get('array');
    this.urlParams = JSON.parse(array);
    console.log(this.urlParams);

    routes.params.subscribe(param => {
      this.userId = param.id;
    });

    this.router.routeReuseStrategy.shouldReuseRoute = () => {
      return false;
    };

    this.mySubscription = this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        // Trick the Router into believing it's last link wasn't previously loaded
        this.router.navigated = false;
      }
    });
    this.cars = new Array<any>();
    this.flightPlusCar.subscriber$.subscribe(data => {
      console.log(data);
      this.flights = data;
    });
   }

  ngOnInit(): void {
    this.loadCars();
    this.flightPlusCar.subscriber$.subscribe(data => {
      console.log(data);
      this.flights = data;
    });
    console.log(this.flights);
  }

  // tslint:disable-next-line:use-lifecycle-interface
  ngOnDestroy() {
    if (this.mySubscription) {
      this.mySubscription.unsubscribe();
    }
  }

  loadCars() {
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
                year: el.year,
                rate: el.rate
              };
              this.cars.push(r);
            });
          }
          console.log(res);
        },
        err => {
          this.toastr.error(err.error, 'Error!');
        }
      );
    }
  }

  onModal(value: boolean) {
    if (value) {
      // carreservation
      const data = {
        from: this.url.from,
        to: this.url.to,
        dep: this.url.dep,
        ret: this.url.ret,
        carId: this.selectedOffer.carId,
        userId: this.userId,
        totalPrice: this.selectedOffer.totalPrice
      };
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
            this.toastr.error(err.error, 'Error!');
          } else {
            this.toastr.error(err.error, 'Error!');
          }
          this.showModal = false;
        }
      );
    } else {
      this.showModal = false;
    }
  }

  onBook(value: any) {
    if (this.userId !== undefined) {
      const data = {
        carId: value,
        from: this.url.from,
        to: this.url.to,
        dep: this.url.dep,
        ret: this.url.ret,
      };
  
      const selectedCar = this.cars.find(x => x.carId === value);
  
      this.selectedOffer = {
        from: this.url.from,
        to: this.url.to,
        dep: this.url.dep,
        ret: this.url.ret,
        brand: selectedCar.brand,
        carId: selectedCar.carId,
        model: selectedCar.model,
        name: selectedCar.name,
        totalPrice: null,
        seatsNumber: selectedCar.seatsNumber,
        type: selectedCar.type,
        year: selectedCar.year
      };
  
      const a = this.carService.getTotalPriceForResevation(data).subscribe(
          (res: any) => {
            this.selectedOffer.totalPrice = res;
            this.showModal = true;
          },
          err => {
            console.log(err);
            this.toastr.error(err.error, 'Error!');
          }
        );
    } else {
      this.showModalError = true;
    }
    

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
    if (this.userId === undefined) {
      this.router.navigate(['/']);
    } else {
      this.router.navigate(['/' + this.userId + '/home']);
    }
  }
  onApplyFilter(value: any) {
    this.loadCars();
    // window.location.reload();
    this.filter = false;
  }

  toggleFilter(value: any) {
    this.filter = !this.filter;
  }

  onModalError(value: any) {
    if (value) {
      this.router.navigate(['/signin']);
    }
    this.showModalError = false;
  }
}
