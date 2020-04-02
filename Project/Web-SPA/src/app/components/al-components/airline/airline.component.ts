import { Component, OnInit, Input } from '@angular/core';
import { AirlineService } from 'src/services/airline.service';

@Component({
  selector: 'app-airline',
  templateUrl: './airline.component.html',
  styleUrls: ['./airline.component.scss']
})
export class AirlineComponent implements OnInit {

  @Input() data;

  colorsOfArilineDest: Array<string>;

  constructor(private airlineService: AirlineService) {
    this.colorsOfArilineDest = new Array<string>();
   }

  ngOnInit(): void {
    this.addColors();
  }

  addColors() {
    this.colorsOfArilineDest.push('#998AD3');
    this.colorsOfArilineDest.push('#E494D3');
    this.colorsOfArilineDest.push('#CDF1AF');
    this.colorsOfArilineDest.push('#87DCC0');
    this.colorsOfArilineDest.push('#88BBE4');
  }

}
