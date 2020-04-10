import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-flight',
  templateUrl: './flight.component.html',
  styleUrls: ['./flight.component.scss']
})
export class FlightComponent implements OnInit {

  option = 'roundTrip';
  showInfo: Array<boolean>;
  flights: Array<any>;
  stops: Array<string>;

  constructor() {
    this.flights = new Array<any>();
    this.stops = new Array<string>();
    this.showInfo = new Array<boolean>();
   }

  ngOnInit(): void {
    this.flights.push(1);
    this.flights.push(1);
    this.showInfo.push(false);
    this.showInfo.push(false);
    this.stops.push('VIE');
  }

  showStopsInfo(i: number) {
    this.showInfo[i] = !this.showInfo[i];
  }

}
