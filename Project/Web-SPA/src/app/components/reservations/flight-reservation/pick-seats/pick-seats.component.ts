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
  @Input() blur: boolean;
  @Output() bookSeat = new EventEmitter<Seat>();

  firstClassPriceRange: {minPrice: number, maxPrice: number};
  businessPriceRange: {minPrice: number, maxPrice: number};
  economyPriceRange: {minPrice: number, maxPrice: number};
  basicEconomyPriceRange: {minPrice: number, maxPrice: number};

  constructor() {
  }

  ngOnInit(): void {
    this.setPriceRange();
  }

  book(seat: Seat) {
    this.bookSeat.emit(seat);
  }

  setPriceRange() {
    this.firstClassPriceRange = {
      minPrice: this.flight.seats.find(x => x.class === 'F').price,
      maxPrice: this.flight.seats.find(x => x.class === 'F').price
    };
    this.flight.seats.forEach(seat => {
      if (seat.class === 'F') {
        if (seat.price < this.firstClassPriceRange.minPrice) {
          this.firstClassPriceRange.minPrice = seat.price;
        }
        if (seat.price > this.firstClassPriceRange.maxPrice) {
          this.firstClassPriceRange.maxPrice = seat.price;
        }
      }
    });

    this.businessPriceRange = {
      minPrice: this.flight.seats.find(x => x.class === 'B').price,
      maxPrice: this.flight.seats.find(x => x.class === 'B').price
    };
    this.flight.seats.forEach(seat => {
      if (seat.class === 'B') {
        if (seat.price < this.businessPriceRange.minPrice) {
          this.businessPriceRange.minPrice = seat.price;
        }
        if (seat.price > this.businessPriceRange.maxPrice) {
          this.businessPriceRange.maxPrice = seat.price;
        }
      }
    });

    this.economyPriceRange = {
      minPrice: this.flight.seats.find(x => x.class === 'E').price,
      maxPrice: this.flight.seats.find(x => x.class === 'E').price
    };
    this.flight.seats.forEach(seat => {
      if (seat.class === 'E') {
        if (seat.price < this.economyPriceRange.minPrice) {
          this.economyPriceRange.minPrice = seat.price;
        }
        if (seat.price > this.economyPriceRange.maxPrice) {
          this.economyPriceRange.maxPrice = seat.price;
        }
      }
    });

    this.basicEconomyPriceRange = {
      minPrice: this.flight.seats.find(x => x.class === 'BE').price,
      maxPrice: this.flight.seats.find(x => x.class === 'BE').price
    };
    this.flight.seats.forEach(seat => {
      if (seat.class === 'BE') {
        if (seat.price < this.basicEconomyPriceRange.minPrice) {
          this.basicEconomyPriceRange.minPrice = seat.price;
        }
        if (seat.price > this.basicEconomyPriceRange.maxPrice) {
          this.basicEconomyPriceRange.maxPrice = seat.price;
        }
      }
    });
  }
}
