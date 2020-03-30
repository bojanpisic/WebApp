import { Component, OnInit } from '@angular/core';
import { RentACarService } from 'src/app/entities/rent-a-car-service';
import { CarRentService } from 'src/services/car-rent.service';

@Component({
  selector: 'app-show-rent-a-car-services',
  templateUrl: './show-rent-a-car-services.component.html',
  styleUrls: ['./show-rent-a-car-services.component.scss', '../airlines/airlines.component.scss']
})
export class ShowRentACarServicesComponent implements OnInit {

  allRentACarServices: Array<RentACarService>;
  colorsOfBranches: Array<string>;

  constructor(private rentService: CarRentService) {
    this.allRentACarServices = new Array<RentACarService>();
    this.colorsOfBranches = new Array<string>();
   }

  ngOnInit(): void {
    this.loadAirlines();
    this.addColors();
  }

  loadAirlines() {
    this.allRentACarServices = this.rentService.loadAllRentServices();
  }

  addColors() {
    this.colorsOfBranches.push('#998AD3');
    this.colorsOfBranches.push('#E494D3');
    this.colorsOfBranches.push('#CDF1AF');
    this.colorsOfBranches.push('#87DCC0');
    this.colorsOfBranches.push('#88BBE4');
  }

}
