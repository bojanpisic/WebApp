import { Component, OnInit } from '@angular/core';
import { Car } from 'src/app/entities/car';
import { Router, ActivatedRoute } from '@angular/router';
import { CarService } from 'src/services/car.service';
import { CarRentService } from 'src/services/car-rent.service';

@Component({
  selector: 'app-rac-cars',
  templateUrl: './rac-cars.component.html',
  styleUrls: ['./rac-cars.component.scss']
})
export class RacCarsComponent implements OnInit {

  searchText = '';
  adminId: number;
  racId: number;
  cars: Array<Car>;

  carIsReserved = false;
  blur = false;

  constructor(private router: Router, private routes: ActivatedRoute, private racService: CarRentService,
              private carService: CarService) {
    routes.params.subscribe(param => {
      this.adminId = param.id;
    });
    this.cars = new Array<Car>();
  }

  ngOnInit(): void {
    this.racId = this.racService.getAdminsRACId(this.adminId);
    this.cars = this.carService.getCarsOfSpecificRAC(this.racId);
  }

  onEdit(value: any) {
    if (this.isReserved()) {
      this.carIsReserved = true;
      this.blur = true;
    } else {
      this.router.navigate(['/rac-admin/' + this.adminId + '/cars/' + value + '/edit-car']);
    }
  }

  onCarIsReserved(value: any) {
    this.carIsReserved = false;
    this.blur = false;
  }

  isReserved() {
    // proveriti da li je auto rezervirano
    return false;
  }

  goBack() {
    this.router.navigate(['/rac-admin/' + this.adminId]);
  }

  focusInput() {
    document.getElementById('searchInput').focus();
  }

}
