import { Component, OnInit, HostListener } from '@angular/core';
import { Airline } from '../entities/airline';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  option = 'fly';
  data: Airline;

  constructor() { }

  ngOnInit(): void {
    console.log(window.innerHeight);
  }

  optionChanged( changedOption: string) {
    this.option = changedOption;
    document.getElementById(this.option).classList.add('choosen-option-class');
    document.getElementById(this.option === 'fly' ? 'drive' : 'fly').classList.remove('choosen-option-class');
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
      console.log(window.scrollY);
      }

}
