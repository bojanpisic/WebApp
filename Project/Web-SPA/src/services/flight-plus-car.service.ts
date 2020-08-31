import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FlightPlusCarService {

  observer = new Subject();
  public subscriber$ = this.observer.asObservable();

  constructor() { }

  emitData(data) {
    this.observer.next(data);
  }
}
