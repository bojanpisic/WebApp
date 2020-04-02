import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-become-a-host',
  templateUrl: './become-a-host.component.html',
  styleUrls: ['./become-a-host.component.scss']
})
export class BecomeAHostComponent implements OnInit {

  @Input() option: string;
  constructor() { }

  ngOnInit(): void {
  }

}
