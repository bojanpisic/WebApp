import { Component, OnInit } from '@angular/core';
import { SpecialOfferService } from 'src/services/special-offer.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SpecialOffer } from 'src/app/entities/special-offer';
import { AirlineService } from 'src/services/airline.service';
import { Airline } from 'src/app/entities/airline';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-special-offers',
  templateUrl: './special-offers.component.html',
  styleUrls: ['./special-offers.component.scss']
})
export class SpecialOffersComponent implements OnInit {

  airlineId: number;
  userId: number;
  airline: Airline;

  specialOffers: Array<{newPrice: number, oldPrice: number, flights: Array<any>}>;
  itsOk = false;

  constructor(private router: Router, private routes: ActivatedRoute, private airlineService: AirlineService,
              private specialOfferService: SpecialOfferService, private san: DomSanitizer) {
    routes.params.subscribe(param => {
      this.airlineId = param.id;
      this.userId = param.user;
    });
    this.specialOffers = [];
  }

  ngOnInit(): void {
    window.scroll(0, 0);
    if (this.airlineId === undefined) {
      this.airlineService.getAllSpecialOffers().subscribe(
        (res: any[]) => {
          if (res.length) {
            console.log(res);
            res.forEach(element => {
              const new1 = {
                newPrice: element.newPrice,
                oldPrice: element.oldPrice,
              };
              const fli = [];
              element.flights.forEach(flight => {
                const st = [];
                flight.stops.forEach(s => {
                  st.push({city: s.city});
                });
                fli.push({
                  // tslint:disable-next-line:max-line-length
                  airlineLogo: (element.logoUrl === null) ? null : this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${element.logoUrl}`),
                  airlineName: element.name,
                  flightId: flight.flightId,
                  from: flight.from,
                  to: flight.to,
                  flightNumber: flight.flightNumber,
                  takeOffDate: flight.takeOffDate,
                  takeOffTime: flight.takeOffTime,
                  landingDate: flight.landingDate,
                  landingTime: flight.landingTime,
                  flightTime: flight.tripTime,
                  flightLength: flight.tripLength,
                  stops: st,
                  seatNum: {column: flight.column, row: flight.row, class: flight.class}
                });
              });
              this.specialOffers.push({newPrice: new1.newPrice, oldPrice: new1.oldPrice, flights: fli});
            });
            this.itsOk = true;
          }
        },
        err => {
          console.log('dada' + err.status);
          // tslint:disable-next-line: triple-equals
          if (err.status == 400) {
            console.log('400' + err);
            // this.toastr.error('Incorrect username or password.', 'Authentication failed.');
          } else if (err.status === 401) {
            console.log(err);
          } else {
            console.log(err);
          }
        }
      );
    } else {
      this.airlineService.getAirlineSpecialOffers(this.airlineId).subscribe(
        (res: any[]) => {
          if (res.length) {
            res.forEach(element => {
              const new1 = {
                newPrice: element.newPrice,
                oldPrice: element.oldPrice,
              };
              const fli = [];
              element.flights.forEach(flight => {
                const st = [];
                flight.stops.forEach(s => {
                  st.push({city: s.city});
                });
                fli.push({
                  // tslint:disable-next-line:max-line-length
                  airlineLogo: (element.logoUrl === null) ? null : this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${element.logoUrl}`),
                  airlineName: element.name,
                  flightId: flight.flightId,
                  from: flight.from,
                  to: flight.to,
                  flightNumber: flight.flightNumber,
                  takeOffDate: flight.takeOffDate,
                  takeOffTime: flight.takeOffTime,
                  landingDate: flight.landingDate,
                  landingTime: flight.landingTime,
                  flightTime: flight.tripTime,
                  flightLength: flight.tripLength,
                  stops: st,
                  seatNum: {column: flight.column, row: flight.row, class: flight.class}
                });
              });
              this.specialOffers.push({newPrice: new1.newPrice, oldPrice: new1.oldPrice, flights: fli});
            });
            this.itsOk = true;
          }
          console.log(res);
        },
        err => {
          console.log('dada' + err.status);
          // tslint:disable-next-line: triple-equals
          if (err.status == 400) {
            console.log('400' + err);
            // this.toastr.error('Incorrect username or password.', 'Authentication failed.');
          } else if (err.status === 401) {
            console.log(err);
          } else {
            console.log(err);
          }
        }
      );
    }
    
  }

  goBack() {
    if (this.userId !== undefined) {
      if (this.airlineId !== undefined) {
        this.router.navigate(['/' + this.userId + '/airlines/' + this.airlineId + '/airline-info']);
      } else {
        this.router.navigate(['/' + this.userId + '/home']);
      }
    } else {
      if (this.airlineId !== undefined) {
        this.router.navigate(['/airlines/' + this.airlineId + '/airline-info']);
      } else {
        this.router.navigate(['/']);
      }
    }
  }

}
