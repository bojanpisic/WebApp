import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-special-offers',
  templateUrl: './special-offers.component.html',
  styleUrls: ['./special-offers.component.scss']
})
export class SpecialOffersComponent implements OnInit {

  tickets: Array<any>;

  constructor() {
    this.tickets = new Array<any>();
  }

  ngOnInit(): void {
    this.tickets.push(1);
    this.tickets.push(1);
    this.tickets.push(1);
  }

}
