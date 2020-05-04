import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Flight } from 'src/app/entities/flight';
import { Passenger } from 'src/app/entities/passenger';
import { SeatsForFlight } from 'src/app/entities/seats-for-flight';
import { Seat } from 'src/app/entities/seat';

@Component({
  selector: 'app-invite-friends',
  templateUrl: './invite-friends.component.html',
  styleUrls: ['./invite-friends.component.scss']
})
export class InviteFriendsComponent implements OnInit {

  @Input() flight: Flight;
  @Output() passengers = new EventEmitter<Array<Passenger>>();
  seats: Array<Seat>;

  constructor() {
    this.seats = new Array<Seat>();
  }

  ngOnInit(): void {
    this.flight.seats.forEach(seat => {
      if (seat.reserved) {
        this.seats.push(seat);
      }
    });
  }

  inviteFriend() {
    const passengers = new Array<Passenger>();
    this.passengers.emit(passengers);
  }

}
