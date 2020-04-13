import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-car',
  templateUrl: './car.component.html',
  styleUrls: ['./car.component.scss']
})
export class CarComponent implements OnInit {

  @Input() car;
  showInfo = false;

  constructor() { }

  ngOnInit(): void {
  }

  showStopsInfo() {
    this.showInfo = !this.showInfo;
  }

}
