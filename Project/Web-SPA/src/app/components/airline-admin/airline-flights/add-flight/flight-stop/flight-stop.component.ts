import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Address } from 'src/app/entities/address';

@Component({
  selector: 'app-flight-stop',
  templateUrl: './flight-stop.component.html',
  styleUrls: ['./flight-stop.component.scss']
})
export class FlightStopComponent implements OnInit {

  errorLocation = false;
  location: Address;
  lastGoodLocationString: string;
  lastLocationString: string;

  @Output() stop = new EventEmitter<Address>();
  @Output() valid = new EventEmitter<boolean>();

  constructor() {}

  ngOnInit(): void {
  }

  onLocation(value: any) {
    this.lastGoodLocationString = this.lastLocationString;
    const obj = JSON.parse(value);
    this.location = new Address(obj.city, obj.state, obj.short_name, obj.longitude, obj.latitude);
    this.stop.emit(this.location);
    this.valid.emit(true);
  }

  onInputChange(value: any) {
    this.lastLocationString = value;
    if (this.lastLocationString !== this.lastGoodLocationString) {
      this.valid.emit(false);
    } else {
      this.valid.emit(true);
    }
  }

  removeErrorClass() {
    this.errorLocation = false;
  }

}
