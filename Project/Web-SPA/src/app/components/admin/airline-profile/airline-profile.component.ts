import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Airline } from './node_modules/src/app/entities/airline';
import { AirlineService } from './node_modules/src/services/airline.service';
import { ActivatedRoute } from '@angular/router';
import { Address } from './node_modules/src/app/entities/address';

@Component({
  selector: 'app-airline-profile',
  templateUrl: './airline-profile.component.html',
  styleUrls: ['./airline-profile.component.scss']
})
export class AirlineProfileComponent implements OnInit {

  adminId: number;
  airline: Airline;

  form: FormGroup;

  location: Address;
  lastLocationString: string;
  lastGoodLocationString: string;

  errorLocation = false;

  constructor(private route: ActivatedRoute, private airlineService: AirlineService) {
    route.params.subscribe(params => {
      this.adminId = params.id;
    });
  }

  ngOnInit(): void {
    this.airline = this.airlineService.getAdminsAirline(this.adminId);
    this.initForm();
  }

  initForm() {
    this.form = new FormGroup({
      name: new FormControl('', Validators.required),
      about: new FormControl(this.airline.about, Validators.required),
    });
  }

  goBack() {

  }

  onSubmit() {
    if (this.validateForm()) {
      console.log(this.airline.about);
    }
  }

  validateForm() {
    let retVal = true;
    if (this.location === undefined && this.lastLocationString === undefined) {
      return retVal;
    }
    if (this.lastLocationString === '') {
      this.location = this.airline.address;
      return retVal;
    }
    if (this.lastGoodLocationString !== this.lastLocationString) {
      this.errorLocation = true;
      retVal = false;
    }
    return retVal;
  }

  onLocation(value: any) {
    const obj = JSON.parse(value);
    this.location = new Address(obj.city, obj.state, obj.short_name, obj.longitude, obj.latitude);
    this.lastGoodLocationString = this.lastLocationString;
  }

  onLocationInputChange(value: any) {
    this.lastLocationString = value;
  }

  removeErrorClass() {
    this.errorLocation = false;
  }
}
