import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CarService } from 'src/services/car.service';

@Component({
  selector: 'app-my-cars',
  templateUrl: './my-cars.component.html',
  styleUrls: ['./my-cars.component.scss']
})
export class MyCarsComponent implements OnInit {

  userId: number;
  cars: Array<any>;

  constructor(private routes: ActivatedRoute, private carService: CarService) {
    routes.params.subscribe(param => {
      this.userId = param.id;
    });
    this.cars = new Array<any>();
  }

  ngOnInit(): void {
    this.cars = this.carService.getAllCars();
  }

  goBack() {
    console.log('bla');
  }

  onRateCar(value: any) {

  }

  onRateService(value: any){
    
  }

}
