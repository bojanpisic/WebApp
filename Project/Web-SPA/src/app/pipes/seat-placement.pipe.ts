import { Pipe, PipeTransform } from '@angular/core';
import { Seat } from '../entities/seat';

@Pipe({
  name: 'seatPlacement'
})
export class SeatPlacementPipe implements PipeTransform {

  transform(seats: Array<Seat>, side?: string, classParam?: string): Array<Seat> {
    const retVal = new Array<Seat>();
    if (side === 'left') {
      seats.forEach(seat => {
        if ((seat.column === 'A' || seat.column === 'B' || seat.column === 'C') && seat.class === classParam) {
          retVal.push(seat);
        }
      });
    } else {
      seats.forEach(seat => {
        if ((seat.column === 'D' || seat.column === 'E' || seat.column === 'F') && seat.class === classParam) {
          retVal.push(seat);
        }
      });
    }
    return retVal;
  }

}
