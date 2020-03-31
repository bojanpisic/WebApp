import { Component, OnInit } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { AirlineService } from 'src/services/airline.service';

@Component({
  selector: 'app-airlines-home',
  templateUrl: './airlines-home.component.html',
  styleUrls: ['./airlines-home.component.scss']
})
export class AirlinesHomeComponent implements OnInit {

  allAirlines: Array<Airline>;
  colorsOfArilineDest: Array<string>;

  constructor(private airlineService: AirlineService) {
    this.allAirlines = new Array<Airline>();
    this.colorsOfArilineDest = new Array<string>();
   }

  ngOnInit(): void {
    this.loadAirlines();
    this.addColors();
  }

  loadAirlines() {
    this.allAirlines = this.airlineService.loadAllAirlines();
  }

  addColors() {
    this.colorsOfArilineDest.push('#998AD3');
    this.colorsOfArilineDest.push('#E494D3');
    this.colorsOfArilineDest.push('#CDF1AF');
    this.colorsOfArilineDest.push('#87DCC0');
    this.colorsOfArilineDest.push('#88BBE4');
  }

}
