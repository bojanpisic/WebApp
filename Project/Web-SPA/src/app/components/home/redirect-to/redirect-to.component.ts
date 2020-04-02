import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-redirect-to',
  templateUrl: './redirect-to.component.html',
  styleUrls: ['./redirect-to.component.scss']
})
export class RedirectToComponent implements OnInit {

  @Input() option: string;

  constructor() { }

  ngOnInit(): void {
  }

}
