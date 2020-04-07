import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-special-offers',
  templateUrl: './special-offers.component.html',
  styleUrls: ['./special-offers.component.scss']
})
export class SpecialOffersComponent implements OnInit {

  option = 'oneWay';
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
    this.showInfo.push(false);
    this.stops.push('VIE');
  }

  showStopsInfo(i: number) {
    this.showInfo[i] = !this.showInfo[i];
  }

}
