import { Component, OnInit, Input } from '@angular/core';
import { AirlineService } from 'src/services/airline.service';
import { Airline } from 'src/app/entities/airline';

@Component({
  selector: 'app-special-offer',
  templateUrl: './special-offer.component.html',
  styleUrls: ['./special-offer.component.scss']
})
export class SpecialOfferComponent implements OnInit {

  @Input() offer;
  airline: Airline;
  showInfo: Array<boolean>;
  i: number;


  constructor(private airlineService: AirlineService) {
    this.showInfo = new Array<boolean>();
  }

  ngOnInit(): void {
    this.i = this.showInfo.length;
    this.showInfo.push(false);
  }
  getAirlineName(flightId: number) {
    this.airline = this.airlineService.getAirline(flightId);
    return this.airline.name;
  }

  showStopsInfo(i: number) {
    this.showInfo[i] = !this.showInfo[i];
  }

}
