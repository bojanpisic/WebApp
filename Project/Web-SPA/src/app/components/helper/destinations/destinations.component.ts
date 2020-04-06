import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-destinations',
  templateUrl: './destinations.component.html',
  styleUrls: ['./destinations.component.scss']
})
export class DestinationsComponent implements OnInit {

  @Input() data: string;
  selected = false;

  constructor() { }

  ngOnInit(): void {
  }

  select() {
    this.selected = !this.selected;
  }

}
