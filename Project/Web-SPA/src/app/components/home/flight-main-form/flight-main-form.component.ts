import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-flight-main-form',
  templateUrl: './flight-main-form.component.html',
  styleUrls: ['./flight-main-form.component.scss']
})
export class FlightMainFormComponent implements OnInit {

  // let.flightType = form.controls['flight-type'].value;
  // u svaki radio input [ngModel]="let.flightType"
  oneWayFlight = false;
  roundTripFlight = false;
  multiCityFlight = true;

  constructor() { }

  addFlight() {
    const ul = document.getElementById('ul');
    ul.append('<li><app-flight-form-part></app-flight-form-part></li>');
  }

  ngOnInit(): void {
    console.log(this.oneWayFlight);
    console.log(this.roundTripFlight);
    console.log(this.multiCityFlight);
  }

  oneWay() {
    if (!this.oneWayFlight) {
      this.oneWayFlight = true;
      this.roundTripFlight = false;
      this.multiCityFlight = false;
    }
  }

  roundTrip() {
    if (!this.roundTripFlight) {
      this.roundTripFlight = true;
      this.oneWayFlight = false;
      this.multiCityFlight = false;
    }
  }

  multiCity() {
    if (!this.multiCityFlight) {
      this.multiCityFlight = true;
      this.roundTripFlight = false;
      this.oneWayFlight = false;
    }
  }

}
