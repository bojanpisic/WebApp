import { Component, OnInit } from '@angular/core';
import { CarService } from 'src/services/car.service';
import { Car } from 'src/app/entities/car';
import { ActivatedRoute } from '@angular/router';
import { RegisteredUser } from 'src/app/entities/registeredUser';
import { UserService } from 'src/services/user.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-cars',
  templateUrl: './cars.component.html',
  styleUrls: ['./cars.component.scss']
})
export class CarsComponent implements OnInit {

  userId: number;
  user: RegisteredUser;
  cars: Array<Car>;

  urlParams = [];

  constructor(private userService: UserService, private carService: CarService,
              private routes: ActivatedRoute, private location: Location) {
    const array = routes.snapshot.queryParamMap.get('array');
    this.urlParams = JSON.parse(array);
    console.log(this.urlParams);

    routes.params.subscribe(param => {
      this.userId = param.id;
    });
    this.cars = new Array<Car>();
   }

  ngOnInit(): void {
    this.cars = this.carService.getAllCars();
  }

  goBack() {
    this.location.back();
  }
}
