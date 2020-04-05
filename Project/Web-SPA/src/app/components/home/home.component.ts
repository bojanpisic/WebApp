import { Component, OnInit, HostListener } from '@angular/core';
import { Airline } from '../../entities/airline';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  fly = true;
  drive = false;
  option = 'fly';

  data: Airline;

  constructor() { }

  ngOnInit(): void {
    console.log(window.innerHeight);
    console.log('fly' + this.fly);
    console.log('drive' + this.drive);
  }

  onFly() {
    if (!this.fly) {
      this.fly = true;
      this.drive = false;
      this.option = 'fly';
    }
  }

  onDrive() {
    if (!this.drive) {
      this.fly = false;
      this.drive = true;
      this.option = 'drive';
    }
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
      console.log(window.scrollY);
      }

}
