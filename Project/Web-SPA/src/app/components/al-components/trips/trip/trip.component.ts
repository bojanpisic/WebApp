import { Component, OnInit, Input } from '@angular/core';
import { AirlineService } from 'src/services/airline.service';

@Component({
  selector: 'app-trip',
  templateUrl: './trip.component.html',
  styleUrls: ['./trip.component.scss']
})
export class TripComponent implements OnInit {

  @Input() trip;
  showInfo: Array<boolean>;
  i: number;


  constructor(private airlineService: AirlineService) {
    this.showInfo = new Array<boolean>();
  }

  ngOnInit(): void {
    this.i = this.showInfo.length;
    this.showInfo.push(false);
  }
  getAirlineName(airlineId: number) {
    return this.airlineService.getAirline(airlineId);
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