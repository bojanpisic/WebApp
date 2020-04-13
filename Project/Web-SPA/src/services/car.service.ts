import { Injectable } from '@angular/core';
import { Car } from 'src/app/entities/car';

@Injectable({
  providedIn: 'root'
})
export class CarService {

  constructor() { }

  getAllCars() { 
    return new Array<Car>()
  }
}
