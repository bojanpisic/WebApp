import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-pick-dates-special-offer',
  templateUrl: './pick-dates-special-offer.component.html',
  styleUrls: ['./pick-dates-special-offer.component.scss']
})
export class PickDatesSpecialOfferComponent implements OnInit {

  public valueTo: Date;
  public valueFrom: Date;
  @Input() carPricePerDay: number;
  @Output() addSpecialOffer = new EventEmitter<{fromDate: string, toDate: string, newPrice: number}>();

  form: FormGroup;

  constructor() { }

  ngOnInit(): void {
    this.initForm();
  }

  public disabledDates = (date: Date): boolean => {
    return date.getDate() % 2 === 0;
  }

  onSubmit() {
    // tslint:disable-next-line:max-line-length
    this.addSpecialOffer.emit({fromDate: this.valueFrom.toDateString(), toDate: this.valueTo.toDateString(), newPrice: this.form.controls.newPrice.value})
  }

  initForm() {
    this.form = new FormGroup({
      newPrice: new FormControl('', Validators.required)
    });
  }

}
