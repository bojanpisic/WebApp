import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RentACarService } from 'src/app/entities/rent-a-car-service';
import { CarRentService } from 'src/services/car-rent.service';
import { Location } from '@angular/common';


@Component({
  selector: 'app-rent-a-car-service-info',
  templateUrl: './rent-a-car-service-info.component.html',
  styleUrls: ['./rent-a-car-service-info.component.scss' , '../../al-components/airline-info/airline-info.component.scss']
})
export class RentACarServiceInfoComponent implements OnInit {

  id: number;
  allServices: Array<RentACarService>;
  choosenRentService: RentACarService;

  constructor(private route: ActivatedRoute, private service: CarRentService, private location: Location) {
    route.params.subscribe(params => { this.id = params.id; });
    this.allServices = new Array<RentACarService>();
  }

  ngOnInit(): void {
    this.allServices = this.service.loadAllRentServices();
    this.choosenRentService = this.allServices[this.id];
  }
  
  goBack() {
    this.location.back();
  }
}
