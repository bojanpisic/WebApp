import { Component, OnInit, Input } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { AirlineService } from 'src/services/airline.service';
import { RentACarService } from 'src/app/entities/rent-a-car-service';
import { CarRentService } from 'src/services/car-rent.service';

@Component({
  selector: 'app-top-rated',
  templateUrl: './top-rated.component.html',
  styleUrls: ['./top-rated.component.scss']
})
export class TopRatedComponent implements OnInit {

  @Input() option: string;

  allRentACarServices: Array<RentACarService>;
  allAirlines: Array<Airline>;

  constructor(private airlineService: AirlineService, private rentService: CarRentService) {
    this.allRentACarServices = new Array<RentACarService>();
    this.allAirlines = new Array<Airline>();
   }

  ngOnInit(): void {
    // this.loadRentACarServices();
    // this.loadAirlines();
    this.allAirlines = [];
    this.allRentACarServices = [];
  }

  loadAirlines() {
    this.allAirlines = this.airlineService.loadAllAirlines();
  }

  loadRentACarServices() {
    const air1 = this.airlineService.getTopRatedAirlines().subscribe(
      (res: any[]) => {
        if (res.length) {
          res.forEach(element => {
            const new1 = {
              // flightId: element.flightId,
              // flightNumber: element.flightNumber,
              // // tslint:disable-next-line:max-line-length
              // tslint:disable-next-line:max-line-length
              // airlineLogo: (element.airlineLogo === null) ? null : this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${element.airlineLogo}`),
              // airlineName: element.airlineName,
              // from: element.from,
              // takeOffDate: element.takeOffDate,
              // takeOffTime: element.takeOffTime,
              // to: element.to,
              // landingDate: element.landingDate,
              // landingTime: element.landingTime,
              // flightLength: element.flightLength,
              // flightTime: element.flightTime,
              // stops: element.stops
            };
            // this.flights.push(new1);
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

}
