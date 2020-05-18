import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Destination } from 'src/app/entities/destination';

@Component({
  selector: 'app-airline-destination',
  templateUrl: './airline-destination.component.html',
  styleUrls: ['./airline-destination.component.scss']
})
export class AirlineDestinationComponent implements OnInit {

  @Input() destination: Destination;
  @Input() editable;
  @Output() delete = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit(): void {
  }

  removeDestination() {
    this.delete.emit(true);
  }

}
