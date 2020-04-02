import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-rent-a-car-service',
  templateUrl: './rent-a-car-service.component.html',
  styleUrls: ['./rent-a-car-service.component.scss', '../../al-components/airline/airline.component.scss']
})
export class RentACarServiceComponent implements OnInit {

  @Input() data;

  colorsOfBranches: Array<string>;

  constructor() {
    this.colorsOfBranches = new Array<string>();
  }

  ngOnInit(): void {
    this.addColors();
  }

  addColors() {
    this.colorsOfBranches.push('#998AD3');
    this.colorsOfBranches.push('#E494D3');
    this.colorsOfBranches.push('#CDF1AF');
    this.colorsOfBranches.push('#87DCC0');
    this.colorsOfBranches.push('#88BBE4');
  }

}
