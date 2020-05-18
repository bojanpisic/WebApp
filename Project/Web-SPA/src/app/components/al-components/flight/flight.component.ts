import { Component, OnInit, Input } from '@angular/core';
import { Flight } from 'src/app/entities/flight';

@Component({
  selector: 'app-flight',
  templateUrl: './flight.component.html',
  styleUrls: ['./flight.component.scss']
})
export class FlightComponent implements OnInit {

  @Input() flight: Flight;
  @Input() markFlightName = false;

  constructor() { }

  ngOnInit(): void {
  }

}
