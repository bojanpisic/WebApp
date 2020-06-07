import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Car } from 'src/app/entities/car';
import { CarRentService } from 'src/services/car-rent.service';
import { CarService } from 'src/services/car.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-edit-car',
  templateUrl: './edit-car.component.html',
  styleUrls: ['./edit-car.component.scss']
})
export class EditCarComponent implements OnInit {

  adminId: number;
  carId: number;
  car: Car;

  seatsNumber = 10;
  pricePerDay = 200;
  minusDisabled = false;

  dropdown = false;
  pickedCarType = 'Luxury';

  form: FormGroup;
  selectedFile: File;

  constructor(private router: Router, private routes: ActivatedRoute, private carService: CarService) {
    routes.params.subscribe(param => {
      this.adminId = param.id;
    });
    routes.params.subscribe(param => {
      this.carId = param.car;
    });
  }

  onPlus() {
    this.seatsNumber++;
    this.minusDisabled = false;
  }

  onMinus() {
    if (this.seatsNumber > 1) {
      this.seatsNumber--;
    }
    if (this.seatsNumber === 1) {
      this.minusDisabled = true;
    }
  }

  onDelete() {

  }

  onConfirm() {

  }

  setCarType(value: string) {
    this.pickedCarType = value;
  }

  onFileChanged(event) {
    this.selectedFile = event.target.files[0];
  }

  toggleDropDown() {
    this.dropdown = !this.dropdown;
    console.log(this.pickedCarType);
  }

  ngOnInit(): void {
    this.car = this.carService.getCar(this.adminId, this.carId);
    this.initForm();
  }

  goBack() {
    this.router.navigate(['/rac-admin/' + this.adminId + '/cars']);
  }

  onSubmit() {

  }

  initForm() {
    this.form = new FormGroup({
      brand: new FormControl('', Validators.required),
      model: new FormControl('', Validators.required),
      year: new FormControl('', Validators.required),
   });
  }

}
