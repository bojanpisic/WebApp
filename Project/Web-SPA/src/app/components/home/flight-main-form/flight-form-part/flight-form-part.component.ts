import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-flight-form-part',
  templateUrl: './flight-form-part.component.html',
  styleUrls: ['./flight-form-part.component.scss', '../flight-form-part/flight-form-part.component.scss']
})
export class FlightFormPartComponent implements OnInit {
  @Input() flightType: any;

  constructor() { }

  ngOnInit(): void {
  }

}
