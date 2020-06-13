import { Component, OnInit, Input } from '@angular/core';
import { Flight } from 'src/app/entities/flight';

@Component({
  selector: 'app-flight',
  templateUrl: './flight.component.html',
  styleUrls: ['./flight.component.scss']
})
export class FlightComponent implements OnInit {

  @Input() flight: {flightId: number,
    flightNumber: string,
    airlineLogo: any,
    airlineName: string,
    from: string,
    takeOffDate: Date,
    takeOffTime: string,
    to: string,
    landingDate: Date,
    landingTime: string,
    flightLength: string,
    flightTime: string,
    stops: Array<any>};
  @Input() markFlightName = false;
  @Input() seat = false;
  @Input() seatName: any;

  constructor() { }

  ngOnInit(): void {
    console.log(this.flight);
  }

}
