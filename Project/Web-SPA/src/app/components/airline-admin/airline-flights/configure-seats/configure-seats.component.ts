import { Component, OnInit } from '@angular/core';
import { FlightService } from 'src/services/flight.service';
import { Flight } from 'src/app/entities/flight';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Seat } from 'src/app/entities/seat';

@Component({
  selector: 'app-configure-seats',
  templateUrl: './configure-seats.component.html',
  styleUrls: ['./configure-seats.component.scss']
})
export class ConfigureSeatsComponent implements OnInit {

  flight: Flight;
  flightId: number;

  pickedSeat: Seat;
  showModify = false;
  blur = false;

  firstClassSeats: Array<any>;
  businessSeats: Array<any>;
  economySeats: Array<any>;
  basicEconomySeats: Array<any>;

  firstClassPriceRange: {minPrice: number, maxPrice: number};
  businessPriceRange: {minPrice: number, maxPrice: number};
  economyPriceRange: {minPrice: number, maxPrice: number};
  basicEconomyPriceRange: {minPrice: number, maxPrice: number};

  constructor(private flightService: FlightService, private route: ActivatedRoute, private location: Location) {
    route.params.subscribe(param => {
      this.flightId = param.flight;
    });

    this.firstClassSeats = new Array<any>();
    this.businessSeats = new Array<any>();
    this.economySeats = new Array<any>();
    this.basicEconomySeats = new Array<any>();
  }

  ngOnInit(): void {
    window.scroll(0, 0);
    this.flight = this.flightService.getFlight(this.flightId);

    this.setPriceRange();

    this.setFirstClassSeats();
    this.setBusinessSeats();
    this.setEconomySeats();
    this.setBasicEconomySeats();

    console.log(this.businessSeats);
  }

  addSeat(index?: number, side?: string, classParam?: string) {
    const row = ((index + 1) % 3 === 0) ? (index + 1) / 3 : Math.floor((index + 1) / 3) + 1;
    let column = '';
    if (side === 'left') {
      column = ((index + 1) % 3 === 1) ? 'A' : ((index + 1) % 3 === 2) ? 'B' : 'C';
    } else {
      column = ((index + 1) % 3 === 1) ? 'D' : ((index + 1) % 3 === 2) ? 'E' : 'F';
      // this.seats.push(new Seat('F', column, row, 400));
    }
    const previousSeat = this.findPreviosSeat(row, column, classParam);
    const insertIndex = this.flight.seats.indexOf(previousSeat);
    console.log(index, side, classParam);
    console.log(previousSeat);
    if (previousSeat !== null) {
      this.flight.seats.splice(insertIndex + 1, 0, new Seat(classParam, column, row, this.setPrice(classParam)));
    } else {
      this.flight.seats.splice(insertIndex + 1, 0, new Seat(classParam, column, row, this.setPrice(classParam)));
    }
    if (classParam === 'F') {
      this.firstClassSeats = [];
      this.setFirstClassSeats();
    } else if (classParam === 'B') {
      this.businessSeats = [];
      this.setBusinessSeats();
    } else if (classParam === 'E') {
      this.economySeats = [];
      this.setEconomySeats();
    } else {
      this.basicEconomySeats = [];
      this.setBasicEconomySeats();
    }
  }

  findPreviosSeat(row: number, column: string, classParam: string) {
    let indexOfSeat = ((row - 1) * 6) + ((column === 'A') ? 0 : (column === 'B') ? 1 : (column === 'C') ? 2 :
                        (column === 'D') ? 3 : (column === 'E') ? 4 : 5);
    while (indexOfSeat > 0) {
      const seat = (classParam === 'F') ? this.firstClassSeats[indexOfSeat] : (classParam === 'B') ? this.businessSeats[indexOfSeat] :
                   (classParam === 'E') ? this.economySeats[indexOfSeat] : this.basicEconomySeats[indexOfSeat];
      if (seat !== 'left' && seat !== 'right') {
        return seat;
      }
      indexOfSeat--;
    }

    return null;
  }

  configure(seat: Seat) {
    this.pickedSeat = seat;
    this.blur = true;
    this.showModify = true;
  }

  onDelete(value: boolean) {
    this.blur = false;
    this.showModify = false;
    if (value) {
      const indexOfSeat = this.flight.seats.indexOf(this.pickedSeat);
      this.flight.seats.splice(indexOfSeat, 1);
      if (this.pickedSeat.class === 'F') {
        this.firstClassSeats = [];
        this.setFirstClassSeats();
      } else if (this.pickedSeat.class === 'B') {
        this.businessSeats = [];
        this.setBusinessSeats();
      } else if (this.pickedSeat.class === 'E') {
        this.economySeats = [];
        this.setEconomySeats();
      } else {
        this.basicEconomySeats = [];
        this.setBasicEconomySeats();
      }
    }
  }

  onChangePrice(value: number) {
    this.blur = false;
    this.showModify = false;
    const index = this.flight.seats.indexOf(this.pickedSeat);
    this.flight.seats[index].price = value;
    if (this.pickedSeat.class === 'F') {
      this.updateFirstClassPriceRange(value);
    } else if (this.pickedSeat.class === 'B') {
      this.updateBusinessPriceRange(value);
    } else if (this.pickedSeat.class === 'E') {
      this.updateEconomyPriceRange(value);
    } else {
      this.updateBasicEconomyPriceRange(value);
    }
  }

  exit() {
    this.location.back();
  }

  setFirstClassSeats() {
    const numberOfSeats = this.flight.seats.length;
    if (this.flight.seats.filter(x => x.class === 'F').length > 0) {
      const rows = this.flight.seats.filter(x => x.class === 'F')[this.flight.seats.filter(x => x.class === 'F').length - 1].row;
      let column;
      for (let r = 1; r < rows + 1; r++) {
        for (let c = 0; c < 6; c++) {
          column = (c === 0) ? 'A' : (c === 1) ? 'B' : (c === 2) ? 'C' : (c === 3) ? 'D' : (c === 4) ? 'E' : 'F';
          if (this.flight.seats.some(seat => seat.row === r && seat.column === column && seat.class === 'F')) {
            this.firstClassSeats.push(this.flight.seats.find(seat => seat.class === 'F' && seat.column === column && seat.row === r));
          } else {
            this.firstClassSeats.push((column === 'A' || column === 'B' || column === 'C') ? 'left' : 'right');
          }
        }
      }
      let value = this.firstClassSeats[this.firstClassSeats.length - 1];
      let lastValue = (value === 'left' || value === 'right') ? value :
                      (value.column === 'F' || value.column === 'A' || value.column === 'B') ? 'left' : 'right';
      while (value === 'left' || value === 'right') {
        lastValue = value;
        this.firstClassSeats.splice(this.firstClassSeats.length - 1, 1);
        value = this.firstClassSeats[this.firstClassSeats.length - 1];
      }
      this.firstClassSeats.push(lastValue);
    }
  }

  setBusinessSeats() {
    const numberOfSeats = this.flight.seats.length;
    if (this.flight.seats.filter(x => x.class === 'B').length > 0) {
      const rows = this.flight.seats.filter(x => x.class === 'B')[this.flight.seats.filter(x => x.class === 'B').length - 1].row;
      let column;
      for (let r = 1; r < rows + 1; r++) {
        for (let c = 0; c < 6; c++) {
          column = (c === 0) ? 'A' : (c === 1) ? 'B' : (c === 2) ? 'C' : (c === 3) ? 'D' : (c === 4) ? 'E' : 'F';
          if (this.flight.seats.some(seat => seat.row === r && seat.column === column && seat.class === 'B')) {
            this.businessSeats.push(this.flight.seats.find(seat => seat.class === 'B' && seat.column === column && seat.row === r));
          } else {
            this.businessSeats.push((column === 'A' || column === 'B' || column === 'C') ? 'left' : 'right');
          }
        }
      }
      let value = this.businessSeats[this.businessSeats.length - 1];
      let lastValue = (value === 'left' || value === 'right') ? value :
                      (value.column === 'F' || value.column === 'A' || value.column === 'B') ? 'left' : 'right';
      while (value === 'left' || value === 'right') {
        lastValue = value;
        this.businessSeats.splice(this.businessSeats.length - 1, 1);
        value = this.businessSeats[this.businessSeats.length - 1];
      }
      this.businessSeats.push(lastValue);
    }
  }

  setEconomySeats() {
    const numberOfSeats = this.flight.seats.length;
    if (this.flight.seats.filter(x => x.class === 'E').length > 0) {
      const rows = this.flight.seats.filter(x => x.class === 'E')[this.flight.seats.filter(x => x.class === 'E').length - 1].row;
      let column;
      for (let r = 1; r < rows + 1; r++) {
        for (let c = 0; c < 6; c++) {
          column = (c === 0) ? 'A' : (c === 1) ? 'B' : (c === 2) ? 'C' : (c === 3) ? 'D' : (c === 4) ? 'E' : 'F';
          if (this.flight.seats.some(seat => seat.row === r && seat.column === column && seat.class === 'E')) {
            this.economySeats.push(this.flight.seats.find(seat => seat.class === 'E' && seat.column === column && seat.row === r));
          } else {
            this.economySeats.push((column === 'A' || column === 'B' || column === 'C') ? 'left' : 'right');
          }
        }
      }
      let value = this.economySeats[this.economySeats.length - 1];
      let lastValue = (value === 'left' || value === 'right') ? value :
                      (value.column === 'F' || value.column === 'A' || value.column === 'B') ? 'left' : 'right';
      while (value === 'left' || value === 'right') {
        lastValue = value;
        this.economySeats.splice(this.economySeats.length - 1, 1);
        value = this.economySeats[this.economySeats.length - 1];
      }
      this.economySeats.push(lastValue);
    }
  }

  setBasicEconomySeats() {
    const numberOfSeats = this.flight.seats.length;
    if (this.flight.seats.filter(x => x.class === 'BE').length > 0) {
      const rows = this.flight.seats.filter(x => x.class === 'BE')[this.flight.seats.filter(x => x.class === 'BE').length - 1].row;
      let column;
      for (let r = 1; r < rows + 1; r++) {
        for (let c = 0; c < 6; c++) {
          column = (c === 0) ? 'A' : (c === 1) ? 'B' : (c === 2) ? 'C' : (c === 3) ? 'D' : (c === 4) ? 'E' : 'F';
          if (this.flight.seats.some(seat => seat.row === r && seat.column === column && seat.class === 'BE')) {
            this.basicEconomySeats.push(this.flight.seats.find(seat => seat.class === 'BE' && seat.column === column && seat.row === r));
          } else {
            this.basicEconomySeats.push((column === 'A' || column === 'B' || column === 'C') ? 'left' : 'right');
          }
        }
      }
      let value = this.basicEconomySeats[this.basicEconomySeats.length - 1];
      let lastValue = (value === 'left' || value === 'right') ? value :
                      (value.column === 'F' || value.column === 'A' || value.column === 'B') ? 'left' : 'right';
      while (value === 'left' || value === 'right') {
        lastValue = value;
        this.basicEconomySeats.splice(this.basicEconomySeats.length - 1, 1);
        value = this.basicEconomySeats[this.basicEconomySeats.length - 1];
      }
      this.basicEconomySeats.push(lastValue);
    }
  }

  setPrice(classParam) {
    let retVal;
    if (classParam === 'F') {
      retVal = this.firstClassPriceRange.minPrice;
    } else if (classParam === 'B') {
      retVal = this.businessPriceRange.minPrice;
    } else if (classParam === 'E') {
      retVal = this.economyPriceRange.minPrice;
    } else {
      retVal = this.basicEconomyPriceRange.minPrice;
    }
    return retVal;
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

  updateFirstClassPriceRange(value: number) {
    if (this.firstClassPriceRange.minPrice > value) {
      this.firstClassPriceRange.minPrice = value;
    }
    if (this.firstClassPriceRange.maxPrice < value) {
      this.firstClassPriceRange.maxPrice = value;
    }
  }

  updateBusinessPriceRange(value: number) {
    if (this.businessPriceRange.minPrice > value) {
      this.businessPriceRange.minPrice = value;
    }
    if (this.businessPriceRange.maxPrice < value) {
      this.businessPriceRange.maxPrice = value;
    }
  }

  updateEconomyPriceRange(value: number) {
    if (this.economyPriceRange.minPrice > value) {
      this.economyPriceRange.minPrice = value;
    }
    if (this.economyPriceRange.maxPrice < value) {
      this.economyPriceRange.maxPrice = value;
    }
  }

  updateBasicEconomyPriceRange(value: number) {
    if (this.basicEconomyPriceRange.minPrice > value) {
      this.basicEconomyPriceRange.minPrice = value;
    }
    if (this.basicEconomyPriceRange.maxPrice < value) {
      this.basicEconomyPriceRange.maxPrice = value;
    }
  }

}
