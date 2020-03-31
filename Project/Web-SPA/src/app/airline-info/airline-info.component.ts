import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AirlineService } from 'src/services/airline.service';
import { Airline } from '../entities/airline';

@Component({
  selector: 'app-airline-info',
  templateUrl: './airline-info.component.html',
  styleUrls: ['./airline-info.component.scss']
})
export class AirlineInfoComponent implements OnInit {

  id: number;
  allAirlines: Array<Airline>;
  choosenAirline: Airline;

  constructor(private route: ActivatedRoute, private airlineService: AirlineService) {
    route.params.subscribe(params => { this.id = params.id; });
    this.allAirlines = new Array<Airline>();
  }

  ngOnInit(): void {
    this.allAirlines = this.airlineService.loadAllAirlines();
    this.choosenAirline = this.allAirlines[this.id];
  }

}
