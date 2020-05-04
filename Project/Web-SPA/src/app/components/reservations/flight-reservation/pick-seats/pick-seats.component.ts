import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Flight } from 'src/app/entities/flight';
import { Seat } from 'src/app/entities/seat';

@Component({
  selector: 'app-pick-seats',
  templateUrl: './pick-seats.component.html',
  styleUrls: ['./pick-seats.component.scss']
})
export class PickSeatsComponent implements OnInit {

  @Input() flight: Flight;
  @Output() bookSeat = new EventEmitter<Seat>();

  constructor() { }

  ngOnInit(): void {
  }

  book(seat: Seat) {
    this.bookSeat.emit(seat);
  }
}
