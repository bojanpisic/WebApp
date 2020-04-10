import { Component, OnInit, Input } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { AirlineService } from 'src/services/airline.service';

@Component({
  selector: 'app-flight',
  templateUrl: './flight.component.html',
  styleUrls: ['./flight.component.scss']
})
export class FlightComponent implements OnInit {

  @Input() flight;
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

  calculateFlightLength(departureTime: string, arrivalTime: string) {
    const departureTimeInMinutes = Number(departureTime.split(':')[0]) * 60 + Number(departureTime.split(':')[1]);
    const arrivalTimeInMinutes = Number(arrivalTime.split(':')[0]) * 60 + Number(arrivalTime.split(':')[1]);

    return Math.floor((arrivalTimeInMinutes - departureTimeInMinutes) / 60) + 'h'
         + Math.floor((arrivalTimeInMinutes - departureTimeInMinutes) % 60) + 'min';
  }

}