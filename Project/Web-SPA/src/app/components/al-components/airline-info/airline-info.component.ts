import { Component, OnInit, Input } from '@angular/core';
import { AirlineService } from 'src/services/airline.service';
import { Airline } from '../../../entities/airline';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-airline-info',
  templateUrl: './airline-info.component.html',
  styleUrls: ['./airline-info.component.scss']
})
export class AirlineInfoComponent implements OnInit {

  id: number;
  allAirlines: Array<Airline>;
  choosenAirline: Airline;

  constructor(private route: ActivatedRoute, private airlineService: AirlineService, private location: Location) {
    route.params.subscribe(params => { this.id = params.id; });
    this.allAirlines = new Array<Airline>();
  }

  ngOnInit(): void {
    this.allAirlines = this.airlineService.loadAllAirlines();
    this.choosenAirline = this.allAirlines[this.id];
  }

  goBack() {
    this.location.back();
  }
}
