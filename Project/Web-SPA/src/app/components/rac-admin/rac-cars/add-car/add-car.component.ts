import { Component, OnInit } from '@angular/core';
import { Car } from 'src/app/entities/car';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CarService } from 'src/services/car.service';

@Component({
  selector: 'app-add-car',
  templateUrl: './add-car.component.html',
  styleUrls: ['./add-car.component.scss']
})
export class AddCarComponent implements OnInit {

  adminId: number;
  carId: number;
  car: Car;

  seatsNumber = 10;
  pricePerDay = 200;
  minusDisabled = false;

  dropdown = false;
  pickedCarType = 'Standard';

  form: FormGroup;
  selectedFile: File;
  imagePreview: string;

  imagePicked = false;
  numberOfSeats = 4;

  errorImage = false;
  errorBrand = false;
  errorModel = false;
  errorYear = false;
  errorSeats = false;
  errorPrice = false;

  constructor(private router: Router, private routes: ActivatedRoute, private carService: CarService) {
    routes.params.subscribe(param => {
      this.adminId = param.id;
    });

    this.car = new Car();
  }

  ngOnInit(): void {
    this.initForm();
  }

  onPlus() {
    this.numberOfSeats++;
    this.minusDisabled = false;
  }

  onMinus() {
    if (this.numberOfSeats > 1) {
      this.numberOfSeats--;
    }
    if (this.numberOfSeats === 1) {
      this.minusDisabled = true;
    }
  }

  onDelete() {

  }

  setCarType(value: string) {
    this.pickedCarType = value;
  }

  onFileChanged(event) {
    this.selectedFile = event.target.files[0];
    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview = reader.result.toString();
    };
    reader.readAsDataURL(this.selectedFile);
    console.log(this.selectedFile);
    console.log(this.imagePreview);
  }

  toggleDropDown() {
    this.dropdown = !this.dropdown;
    console.log(this.pickedCarType);
  }

  goBack() {
    this.router.navigate(['/rac-admin/' + this.adminId + '/cars']);
  }

  onSubmit() {
    if (this.validateForm()) {
      // dodaj auto
    }
  }

  validateForm() {
    let retVal = true;
    if (!this.imagePicked) {
      this.errorImage = true;
      retVal = false;
    }
    if (this.form.controls.brand.value === '') {
      this.errorBrand = true;
      retVal = false;
    }
    if (this.form.controls.model.value === '') {
      this.errorModel = true;
      retVal = false;
    }
    if (this.form.controls.year.value === '') {
      this.errorYear = true;
      retVal = false;
    }
    if (this.form.controls.price.value === '') {
      this.errorPrice = true;
      retVal = false;
    }
    return retVal;
  }

  initForm() {
    this.form = new FormGroup({
      brand: new FormControl('', Validators.required),
      model: new FormControl('', Validators.required),
      year: new FormControl('', Validators.required),
      seats: new FormControl('5', Validators.required),
      price: new FormControl('50', Validators.required),
   });
  }

}
