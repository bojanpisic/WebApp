import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Flight } from 'src/app/entities/flight';
import { AirlineService } from 'src/services/airline.service';
import { FlightService } from 'src/services/flight.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-airline-flights',
  templateUrl: './airline-flights.component.html',
  styleUrls: ['./airline-flights.component.scss']
})
export class AirlineFlightsComponent implements OnInit {

  searchText = '';
  adminId: number;
  airlineId: number;
  flights: Array<{
    flightId: number,
    flightNumber: string,
    airlineLogo: any,
    airlineName: string,
    from: string,
    takeOffDate: Date,
    takeOffTime: string,
    to: string,
    landingDate: Date,
    landingTime: string,
    flightLength: string,
    flightTime: string,
    stops: Array<any>
  }>;
  flightId;

  constructor(private router: Router, private routes: ActivatedRoute, private airlineService: AirlineService,
              private flightService: FlightService, private san: DomSanitizer) {
    routes.params.subscribe(param => {
      this.adminId = param.id;
    });
    this.flights = [];
  }

  ngOnInit(): void {
    const air1 = this.airlineService.getAdminsFlights().subscribe(
      (res: any[]) => {
        if (res.length) {
          res.forEach(element => {
            const new1 = {
              flightId: element.flightId,
              flightNumber: element.flightNumber,
              // tslint:disable-next-line:max-line-length
              airlineLogo: (element.airlineLogo === null) ? null : this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${element.airlineLogo}`),
              airlineName: element.airlineName,
              from: element.from,
              takeOffDate: element.takeOffDate,
              takeOffTime: element.takeOffTime,
              to: element.to,
              landingDate: element.landingDate,
              landingTime: element.landingTime,
              flightLength: element.flightLength,
              flightTime: element.flightTime,
              stops: element.stops
            };
            this.flights.push(new1);
          });
          console.log(res);
        }
        console.log('ok');
        // this.airlineId = res[0].airlineId;
        // this.flights = res[0].flights;
      },
      err => {
        console.log('dada' + err.status);
        // tslint:disable-next-line: triple-equals
        if (err.status == 400) {
          console.log(err);
        // tslint:disable-next-line: triple-equals
        } else if (err.status == 401) {
          console.log(err);
        } else {
          console.log(err);
        }
      }
    );
  }

  goBack() {
    this.router.navigate(['/admin/' + this.adminId]);
  }

  focusInput() {
    document.getElementById('searchInput').focus();
  }

}
