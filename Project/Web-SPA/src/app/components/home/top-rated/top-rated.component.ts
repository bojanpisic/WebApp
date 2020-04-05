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

  constructor(private airlineService: AirlineService, private rentService: CarRentService) {
    this.allRentACarServices = new Array<RentACarService>();
    this.allAirlines = new Array<Airline>();
   }

  ngOnInit(): void {
    this.loadRentACarServices();
    this.loadAirlines();
  }

  loadAirlines() {
    this.allAirlines = this.airlineService.loadAllAirlines();
  }

  loadRentACarServices() {
    this.allRentACarServices = this.rentService.loadAllRentServices();
  }

}
