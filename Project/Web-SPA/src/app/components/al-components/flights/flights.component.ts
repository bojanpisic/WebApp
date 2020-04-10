import { Component, OnInit } from '@angular/core';
import { Flight } from 'src/app/entities/flight';
import { Airline } from 'src/app/entities/airline';
import { FlightService } from 'src/services/flight.service';
import { ActivatedRoute } from '@angular/router';
import { FlightComponent } from '../flight/flight.component';

@Component({
  selector: 'app-flights',
  templateUrl: './flights.component.html',
  styleUrls: ['./flights.component.scss']
})
export class FlightsComponent implements OnInit {

  airlineId: number;
  airline: Airline;
  flights: Array<Flight>;
  anotherOneClicked = false;

  constructor(private route: ActivatedRoute, private flightService: FlightService) {
    route.params.subscribe(params => {
      this.airlineId = params.id;
    });
    this.flights = new Array<Flight>();
   }

  ngOnInit(): void {
    // ovo je odradjeno da se izlistaju za odredjenu aviokompaniju
    // drugu funkciju cemo pozivati kad odabere da vidi sve letove svih kompanija
    // i na sve to treba filtere primeniti
    this.flights = this.flightService.getFlightsOfSpecificAirline(0);
    console.log(this.flights.length + 'duzina');
  }
}
