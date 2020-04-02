import { Component, OnInit, Input } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { AirlineService } from 'src/services/airline.service';
import { RentACarService } from 'src/app/entities/rent-a-car-service';
import { CarRentService } from 'src/services/car-rent.service';

@Component({
  selector: 'app-top-rated',
  templateUrl: './top-rated.component.html',
  styleUrls: ['./top-rated.component.scss']
})
export class TopRatedComponent implements OnInit {

  @Input() option: string;

  allRentACarServices: Array<RentACarService>;
  allAirlines: Array<Airline>;
  colorsOfArilineDest: Array<string>;

  constructor(private airlineService: AirlineService, private rentService: CarRentService) {
    this.allRentACarServices = new Array<RentACarService>();
    this.allAirlines = new Array<Airline>();
    this.colorsOfArilineDest = new Array<string>();
   }

  ngOnInit(): void {
    this.loadRentACarServices();
    this.loadAirlines();
    this.addColors();
  }

  loadAirlines() {
    this.allAirlines = this.airlineService.loadAllAirlines();
  }

  loadRentACarServices() {
    this.allRentACarServices = this.rentService.loadAllRentServices();
  }

  addColors() {
    this.colorsOfArilineDest.push('#998AD3');
    this.colorsOfArilineDest.push('#E494D3');
    this.colorsOfArilineDest.push('#CDF1AF');
    this.colorsOfArilineDest.push('#87DCC0');
    this.colorsOfArilineDest.push('#88BBE4');
  }

}
