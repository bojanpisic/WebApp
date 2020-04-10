import { Component, OnInit } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { AirlineService } from 'src/services/airline.service';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent implements OnInit {

  flightType: string;
  filterForm = true;
  slidingMinPriceValue: number;
  slidingMaxPriceValue: number;
  slidingMinDurationValue: number;
  slidingMaxDurationValue: number;
  minDuration: string;
  maxDuration: string;
  airlinesButton: string;
  allAirlines: Array<Airline>;
  checkedAirlines: Array<boolean>;

  constructor(private airlineService: AirlineService) {
    this.slidingMinPriceValue = 25;
    this.slidingMaxPriceValue = 90;
    this.slidingMinDurationValue = 10;
    this.slidingMaxDurationValue = 75;
    this.allAirlines = new Array<Airline>();
    this.checkedAirlines = new Array<boolean>();
  }

  ngOnInit(): void {
    this.flightType = 'roundTripFlight';
    this.loadAirlines();
  }
  getLeftStylePrice() {
    return this.slidingMinPriceValue + '%';
  }

  getRightStylePrice() {
    return (100 - +this.slidingMaxPriceValue).toString() + '%';
  }

  leftValuePrice() {
    return Math.min(+this.slidingMinPriceValue, +this.slidingMaxPriceValue) - 1;
  }

  rightValuePrice() {
    return Math.max(+this.slidingMinPriceValue, +this.slidingMaxPriceValue) + 1;
  }

  getLeftStyleDuration() {
    return this.slidingMinDurationValue + '%';
  }

  getRightStyleDuration() {
    return (100 - +this.slidingMaxDurationValue).toString() + '%';
  }

  leftValueDuration() {
    this.minDuration = Math.floor(this.slidingMinDurationValue * 24 / 60) + 'h ' + this.slidingMinDurationValue * 24 % 60 + 'min';
    return Math.min(+this.slidingMinDurationValue, +this.slidingMaxDurationValue) - 1;
  }

  rightValueDuration() {
    this.maxDuration = Math.floor(this.slidingMaxDurationValue * 24 / 60) + 'h ' + this.slidingMaxDurationValue * 24 % 60 + 'min';
    return Math.max(+this.slidingMinDurationValue, +this.slidingMaxDurationValue) + 1;
  }

  allAirlinesButton() {
    this.checkedAirlines.forEach((v, i, a) => a[i] = true);
  }

  loadAirlines() {
    this.allAirlines = this.airlineService.loadAllAirlines();
    for (let item of this.allAirlines) {
      this.checkedAirlines.push(true);
    }
  }

  toggleAirlineCheckBox(index: number) {
    this.checkedAirlines[index] = !this.checkedAirlines[index];
  }
}
