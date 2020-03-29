import { Component, OnInit, HostListener } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    console.log(window.innerHeight);
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
      console.log(window.scrollY);
      }

}
