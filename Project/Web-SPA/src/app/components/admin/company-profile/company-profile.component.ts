import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Airline } from 'src/app/entities/airline';
import { AirlineService } from 'src/services/airline.service';
import { ActivatedRoute } from '@angular/router';
import { Address } from 'src/app/entities/address';
import { CarRentService } from 'src/services/car-rent.service';

@Component({
  selector: 'app-company-profile',
  templateUrl: './company-profile.component.html',
  styleUrls: ['./company-profile.component.scss']
})
export class CompanyProfileComponent implements OnInit {

  companyType: string;

  adminId: number;

  companyFields: {name: string, location: Address, about: string};

  form: FormGroup;

  location: Address;
  lastLocationString: string;
  lastGoodLocationString: string;

  errorLocation = false;

  constructor(private route: ActivatedRoute, private airlineService: AirlineService, private racService: CarRentService) {
    route.params.subscribe(params => {
      this.adminId = params.id;
      this.companyType = params.type;
    });
  }

  ngOnInit(): void {
    if (this.companyType === 'airline-profile') {
      const airline = this.airlineService.getAdminsAirline(this.adminId);
      this.companyFields = {
        name: airline.name,
        location: airline.address,
        about: airline.about
      };
    } else if (this.companyType === 'rac-profile') {
      const rac = this.racService.getAdminsRac(this.adminId);
      console.log(rac);
      this.companyFields = {
        name: rac.name,
        location: rac.address,
        about: rac.about
      };
    }
    this.initForm();
  }

  initForm() {
    this.form = new FormGroup({
      name: new FormControl(this.companyFields.name, Validators.required),
      about: new FormControl(this.companyFields.about, Validators.required),
    });
  }

  goBack() {

  }

  onSubmit() {
    if (this.validateForm()) {
      console.log(this.companyFields.about);
    }
  }

  validateForm() {
    let retVal = true;
    if (this.location === undefined && this.lastLocationString === undefined) {
      return retVal;
    }
    if (this.lastLocationString === '') {
      this.location = this.companyFields.location;
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
