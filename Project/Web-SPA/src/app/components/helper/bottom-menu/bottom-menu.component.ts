import { Component, OnInit, HostListener } from '@angular/core';

@Component({
  selector: 'app-bottom-menu',
  templateUrl: './bottom-menu.component.html',
  styleUrls: ['./bottom-menu.component.scss']
})
export class BottomMenuComponent implements OnInit {

  scrolled =  false;
  lastPosition: number = window.innerHeight;

  constructor() { }

  ngOnInit(): void {
    this.scrolled =  true;
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
      if (this.scrolled === true) {
        if (this.lastPosition > window.scrollY + window.innerHeight) {
          this.scrolled = true;
          this.lastPosition = window.scrollY + window.innerHeight;
        } else {
          this.scrolled = false;
        }
      } else {
        if (this.lastPosition > window.scrollY + window.innerHeight) {
          this.scrolled = true;
        } else {
          this.scrolled = false;
          this.lastPosition = window.scrollY + window.innerHeight;
        }
      }
    }
}
