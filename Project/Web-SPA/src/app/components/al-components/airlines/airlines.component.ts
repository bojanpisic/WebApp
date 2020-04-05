import { Component, OnInit } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { AirlineService } from 'src/services/airline.service';

@Component({
  selector: 'app-airlines',
  templateUrl: './airlines.component.html',
  styleUrls: ['./airlines.component.scss']
})
export class AirlinesComponent implements OnInit {

  allAirlines: Array<Airline>;
  colorsOfArilineDest: Array<string>;
  rotateArrow = false;
  nameup = false;
  namedown = false;
  cityup = false;
  citydown = false;
  scrolledY: number;

  constructor(private airlineService: AirlineService) {
    this.allAirlines = new Array<Airline>();
    this.colorsOfArilineDest = new Array<string>();
   }

  ngOnInit(): void {
    this.loadAirlines();
    this.addColors();
  }

  loadAirlines() {
    this.allAirlines = this.airlineService.loadAllAirlines();
  }

  addColors() {
    this.colorsOfArilineDest.push('#998AD3');
    this.colorsOfArilineDest.push('#E494D3');
    this.colorsOfArilineDest.push('#CDF1AF');
    this.colorsOfArilineDest.push('#87DCC0');
    this.colorsOfArilineDest.push('#88BBE4');
  }

  sortClick() {
    this.rotateArrow = this.rotateArrow === true ? false : true;
    this.scrolledY = window.scrollY;
  }

  // list.sort((a, b) => (a.color > b.color) ? 1 : (a.color === b.color) ? ((a.size > b.size) ? 1 : -1) : -1 )

  sortBy() {
    if (this.namedown) {
      if (!this.cityup && !this.citydown) {
        this.allAirlines.sort((a, b) => (a.name > b.name) ? 1 : -1);
      } else if (this.citydown) {
        this.allAirlines.sort((a, b) => (a.name > b.name) ? 1 : (a.name === b.name) ? ((a.address > b.address) ? 1 : -1) : -1);
      } else {
        this.allAirlines.sort((a, b) => (a.name > b.name) ? 1 : (a.name === b.name) ? ((a.address < b.address) ? 1 : -1) : -1);
      }
    } else {
      if (this.nameup) {
        if (!this.cityup && !this.citydown) {
          this.allAirlines.sort((a, b) => (a.name < b.name) ? 1 : -1);
        } else if (this.cityup) {
          this.allAirlines.sort((a, b) => (a.name < b.name) ? 1 : (a.name === b.name) ? ((a.address < b.address) ? 1 : -1) : -1);
        } else {
          this.allAirlines.sort((a, b) => (a.name < b.name) ? 1 : (a.name === b.name) ? ((a.address > b.address) ? 1 : -1) : -1);
        }
      }
    }
  }

  namedownClicked() {
    if (this.nameup === true) {
      this.nameup = false;
    }
    this.namedown = !this.namedown;
    this.sortBy();
  }

  nameupClicked() {
    if (this.namedown === true) {
      this.namedown = false;
    }
    this.nameup = !this.nameup;
    this.sortBy();
  }

  citydownClicked() {
    if (this.cityup === true) {
      this.cityup = false;
    }
    this.citydown = !this.citydown;
    this.sortBy();
  }

  cityupClicked() {
    if (this.citydown === true) {
      this.citydown = false;
    }
    this.cityup = !this.cityup;
    this.sortBy();
  }
}
