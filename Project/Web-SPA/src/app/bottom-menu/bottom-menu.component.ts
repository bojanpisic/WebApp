import { Component, OnInit, HostListener } from '@angular/core';

@Component({
  selector: 'app-bottom-menu',
  templateUrl: './bottom-menu.component.html',
  styleUrls: ['./bottom-menu.component.scss']
})
export class BottomMenuComponent implements OnInit {

  scrolled =  false;

  constructor() { }

  ngOnInit(): void {
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
      this.scrolled = (window.pageYOffset > 8 ) ? true : false;
      }
}
