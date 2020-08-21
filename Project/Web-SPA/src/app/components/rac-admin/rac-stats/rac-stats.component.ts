import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CarRentService } from 'src/services/car-rent.service';
import {Chart} from 'chart.js';

@Component({
  selector: 'app-rac-stats',
  templateUrl: './rac-stats.component.html',
  styleUrls: ['./rac-stats.component.scss']
})
export class RacStatsComponent implements OnInit {

  adminId;
  types: Array<any>;
  dropdown = false;
  pickedType;
  from = '2020-08-30';
  to = '2020-10-30';
  cars: Array<any>;

  constructor(private router: Router,
              private routes: ActivatedRoute,
              private carService: CarRentService,
              private toastr: ToastrService) {
    routes.params.subscribe(route => {
      this.adminId = route.id;
    });
    this.cars = new Array<any>();
    this.types = new Array<any>();
    this.types.push({
      type: 'free', displayedType: 'Free vehicles'
    });
    this.types.push({
      type: 'rented', displayedType: 'Rented vehicles'
    });
    this.pickedType = this.types[0];
  }

  ngOnInit(): void {
    this.getValues();
  }

  getValues() {
    const data = {
      month: this.from,
      to: this.to,
      isFree: (this.pickedType.type === 'free' ? true : false)
    };
    // const a = this.carService.getStats(data).subscribe(
    //   (res: any) => {
    //     if (res.length > 0) {
    //       res.forEach(el => {
    //         const r = {
    //           brand: el.brand,
    //           carId: el.carId,
    //           city: el.city,
    //           model: el.model,
    //           name: el.name,
    //           pricePerDay: el.pricePerDay,
    //           seatsNumber: el.seatsNumber,
    //           state: el.state,
    //           type: el.type,
    //           year: el.year,
    //           rate: el.rate
    //         };
    //         this.cars.push(r);
    //       });
    //   }
    // },
    // err => {
    //   this.toastr.error(err.statusText, 'Error.');
    // });
  }

  onExit() {
    this.router.navigate(['/admin/' + this.adminId]);
  }

  setType(value: any) {
    this.pickedType = value;
    this.getValues();
  }

  toggleDropDown() {
    this.dropdown = !this.dropdown;
  }
}
