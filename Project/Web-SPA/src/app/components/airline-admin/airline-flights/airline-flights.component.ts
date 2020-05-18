import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Flight } from 'src/app/entities/flight';
import { AirlineService } from 'src/services/airline.service';
import { FlightService } from 'src/services/flight.service';

@Component({
  selector: 'app-airline-flights',
  templateUrl: './airline-flights.component.html',
  styleUrls: ['./airline-flights.component.scss']
})
export class AirlineFlightsComponent implements OnInit {

  searchText = '';
  adminId: number;
  airlineId: number;
  flights: Array<Flight>;

  constructor(private router: Router, private routes: ActivatedRoute, private airlineService: AirlineService,
              private flightService: FlightService) {
    routes.params.subscribe(param => {
      this.adminId = param.id;
    });
    this.flights = new Array<Flight>();
  }

  ngOnInit(): void {
    this.airlineId = this.airlineService.getAdminsAirlineId(this.adminId);
    this.flights = this.flightService.getFlightsOfSpecificAirline(this.airlineId);
  }

  goBack() {
    this.router.navigate(['/admin/' + this.adminId]);
  }

  focusInput() {
    document.getElementById('searchInput').focus();
  }

}
