import { Component, OnInit } from '@angular/core';
import { RentACarService } from 'src/app/entities/rent-a-car-service';
import { CarRentService } from 'src/services/car-rent.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-rent-a-car-services',
  templateUrl: './rent-a-car-services.component.html',
  styleUrls: ['./rent-a-car-services.component.scss', '../../al-components/airlines/airlines.component.scss']
})
export class RentACarServicesComponent implements OnInit {

  allRentServices: Array<RentACarService>;
  colorsOfBranches: Array<string>;
  rotateArrow = false;

  nameup = false;
  namedown = false;
  cityup = false;
  citydown = false;

  constructor(private service: CarRentService, private location: Location) {
    this.allRentServices = new Array<RentACarService>();
    this.colorsOfBranches = new Array<string>();
   }

  ngOnInit(): void {
    this.allRentServices = this.service.loadAllRentServices();
  }

  goBack() {
    this.location.back();
  }

  addColors() {
    this.colorsOfBranches.push('#998AD3');
    this.colorsOfBranches.push('#E494D3');
    this.colorsOfBranches.push('#CDF1AF');
    this.colorsOfBranches.push('#87DCC0');
    this.colorsOfBranches.push('#88BBE4');
  }

  sortClick() {
    this.rotateArrow = this.rotateArrow === true ? false : true;
  }

  // list.sort((a, b) => (a.color > b.color) ? 1 : (a.color === b.color) ? ((a.size > b.size) ? 1 : -1) : -1 )

  sortBy() {
    // if (this.namedown) {
    //   if (!this.cityup && !this.citydown) {
    //     this.allAirlines.sort((a, b) => (a.name > b.name) ? 1 : -1);
    //   } else if (this.citydown) {
    //     this.allAirlines.sort((a, b) => (a.name > b.name) ? 1 : (a.name === b.name) ? ((a.address > b.address) ? 1 : -1) : -1);
    //   } else {
    //     this.allAirlines.sort((a, b) => (a.name > b.name) ? 1 : (a.name === b.name) ? ((a.address < b.address) ? 1 : -1) : -1);
    //   }
    // } else {
    //   if (this.nameup) {
    //     if (!this.cityup && !this.citydown) {
    //       this.allAirlines.sort((a, b) => (a.name < b.name) ? 1 : -1);
    //     } else if (this.cityup) {
    //       this.allAirlines.sort((a, b) => (a.name < b.name) ? 1 : (a.name === b.name) ? ((a.address < b.address) ? 1 : -1) : -1);
    //     } else {
    //       this.allAirlines.sort((a, b) => (a.name < b.name) ? 1 : (a.name === b.name) ? ((a.address > b.address) ? 1 : -1) : -1);
    //     }
    //   }
    // }
  }

  namedownClicked() {
    if (this.nameup === true) {
      this.nameup = false;
    }
    this.namedown = !this.namedown;
    this.sortBy();
  }

  nameupClicked() {
    if (this.namedown === true) {
      this.namedown = false;
    }
    this.nameup = !this.nameup;
    this.sortBy();
  }

  citydownClicked() {
    if (this.cityup === true) {
      this.cityup = false;
    }
    this.citydown = !this.citydown;
    this.sortBy();
  }

  cityupClicked() {
    if (this.citydown === true) {
      this.citydown = false;
    }
    this.cityup = !this.cityup;
    this.sortBy();
  }
}
