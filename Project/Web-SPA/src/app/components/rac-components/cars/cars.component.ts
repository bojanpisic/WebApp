import { Component, OnInit } from '@angular/core';
import { CarService } from 'src/services/car.service';
import { Car } from 'src/app/entities/car';

@Component({
  selector: 'app-cars',
  templateUrl: './cars.component.html',
  styleUrls: ['./cars.component.scss']
})
export class CarsComponent implements OnInit {

  cars: Array<Car>;

  constructor(private carService: CarService) {
    this.cars = new Array<Car>();
   }

  ngOnInit(): void {
    this.cars = this.carService.getAllCars();
    this.cars.push(new Car());
  }
}
