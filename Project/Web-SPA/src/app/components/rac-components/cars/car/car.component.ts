import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Car } from 'src/app/entities/car';

@Component({
  selector: 'app-car',
  templateUrl: './car.component.html',
  styleUrls: ['./car.component.scss']
})
export class CarComponent implements OnInit {

  @Input() car: Car;
  @Input() customerView: boolean;
  @Output() editButtonClicked = new EventEmitter<number>();

  constructor() { }

  ngOnInit(): void {
  }

  onEdit() {
    this.editButtonClicked.emit(this.car.id);
  }

}
