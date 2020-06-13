import { Component, OnInit, Input } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Address } from 'src/app/entities/address';

@Component({
  selector: 'app-drive-main-form',
  templateUrl: './drive-main-form.component.html',
  styleUrls: ['./drive-main-form.component.scss', '../flight-main-form/flight-main-form.component.scss']
})
export class DriveMainFormComponent implements OnInit {

  sameLocationChoosed = true;
  @Input() userId;

  date: string;
  pickUpLocation: Address;
  dropOffLocation: Address;

  lastGoodPickUpLocation: string;
  lastPickUpLocation: string;
  lastGoodDropOffLocation: string;
  lastDropOffLocation: string;

  errorPickUpLocation = false;
  errorDropOffLocation = false;
  errorPickUpDate = false;
  errorDropOffDate = false;

  form: FormGroup;

  constructor(private router: Router) { }

  ngOnInit(): void {
    this.initForm();
  }

  sameLocation() {
    this.sameLocationChoosed = true;
  }

  differentLocation() {
    this.sameLocationChoosed = false;
  }

  onPickUpLocation(value: any) {
    const obj = JSON.parse(value);
    this.pickUpLocation = new Address(obj.city, obj.state, obj.longitude, obj.latitude);
    this.lastGoodPickUpLocation = this.lastPickUpLocation;
  }

  onPickUpLocationInputChange(value: any) {
    this.lastPickUpLocation = value;
  }

  onDropOffLocation(value: any) {
    const obj = JSON.parse(value);
    this.dropOffLocation = new Address(obj.city, obj.state, obj.longitude, obj.latitude);
    this.lastGoodDropOffLocation = this.lastDropOffLocation;
  }

  onDropOffLocationInputChange(value: any) {
    this.lastDropOffLocation = value;
  }

  validateSameLocationForm() {
    let retVal = true;
    if (this.pickUpLocation === undefined || this.lastGoodPickUpLocation !== this.lastPickUpLocation) {
      this.errorPickUpLocation = true;
      retVal = false;
    }
    if (!this.sameLocationChoosed) {
      if (!this.validateDifferentLocationForm()) {
        this.errorDropOffLocation = true;
        retVal = false;
      }
    }
    if (this.form.controls.pickupDate.value === '') {
      this.errorPickUpDate = true;
      retVal = false;
    }
    if (this.form.controls.dropoffDate.value === '') {
      this.errorDropOffDate = true;
      retVal = false;
    }
    return retVal;
  }

  validateDifferentLocationForm() {
    if (this.dropOffLocation === undefined || this.lastGoodDropOffLocation !== this.lastDropOffLocation) {
      return false;
    }
    return true;
  }

  removeErrorClassPickUp() {
    this.errorPickUpLocation = false;
  }

  removeErrorClassDropOff() {
    this.errorDropOffLocation = false;
  }

  onSubmit() {
    if (this.validateSameLocationForm()) {
      const queryParams: any = {};
      const array = [];
      array.push({pcity: this.pickUpLocation.city, pstate: this.pickUpLocation.state});
      if (!this.sameLocationChoosed) {
        array.push({dcity: this.dropOffLocation.city, dstate: this.dropOffLocation.state});
      }
      array.push({pdate: this.form.controls.pickupDate.value});
      array.push({ddate: this.form.controls.dropoffDate.value});

      queryParams.array = JSON.stringify(array);

      const navigationExtras: NavigationExtras = {
        queryParams
      };
      if (this.userId !== undefined) {
        this.router.navigate(['/' + this.userId + '/cars'], navigationExtras);
      } else {
        this.router.navigate(['/cars'], navigationExtras);
      }
    } else {
      console.log('error');
    }
  }

  initForm() {
    this.form = new FormGroup({
      pickupDate: new FormControl('', Validators.required),
      dropoffDate: new FormControl('', Validators.required),
   });
  }
}
