import { Component, OnInit, Input } from '@angular/core';
import { AirlineService } from 'src/services/airline.service';
import { Airline } from '../../../entities/airline';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Destination } from 'src/app/entities/destination';

@Component({
  selector: 'app-airline-info',
  templateUrl: './airline-info.component.html',
  styleUrls: ['./airline-info.component.scss']
})
export class AirlineInfoComponent implements OnInit {

  id: number;
  allAirlines: Array<Airline>;
  choosenAirline: Airline;
  destinations: Array<Destination>;
  rotateArrow = false;
  buttonContent = 'Paris';

  constructor(private route: ActivatedRoute, private airlineService: AirlineService, private location: Location) {
    route.params.subscribe(params => { this.id = params.id; });
    this.allAirlines = new Array<Airline>();
    this.destinations = new Array<Destination>();
  }

  ngOnInit(): void {
    window.scroll(0, 0);
    this.allAirlines = this.airlineService.loadAllAirlines();
    this.choosenAirline = this.allAirlines[this.id];
    this.destinations = this.choosenAirline.flightDestionations;
    console.log('radi bez problema');
  }

  destinationsClick() {
    this.rotateArrow = this.rotateArrow === true ? false : true;

    if (this.rotateArrow) {
      document.getElementById('destinations').classList.remove('hide-destinations');
      document.getElementById('destinations').classList.add('show-destinations');
    } else {
      document.getElementById('destinations').classList.add('hide-destinations');
      document.getElementById('destinations').classList.remove('show-destinations');
    }
  }

  goBack() {
    this.location.back();
  }
}
