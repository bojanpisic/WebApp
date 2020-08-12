import { Component, OnInit, Input } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { AirlineService } from 'src/services/airline.service';
import { RentACarService } from 'src/app/entities/rent-a-car-service';
import { CarRentService } from 'src/services/car-rent.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-top-rated',
  templateUrl: './top-rated.component.html',
  styleUrls: ['./top-rated.component.scss']
})
export class TopRatedComponent implements OnInit {

  @Input() option: string;

  allRentACarServices: Array<any>;
  allAirlines: Array<any>;

  constructor(private airlineService: AirlineService, private rentService: CarRentService, private san: DomSanitizer) {
    this.allAirlines = [];
    this.allRentACarServices = [];
   }

  ngOnInit(): void {
    // this.loadRentACarServices();
    // this.loadAirlines();
    this.loadAirlines();
  }

  loadAirlines() {
    let count = 0;
    // const a = this.airlineService.getAirlines().subscribe(
    //   (res: any[]) => {
    //     if (res.length > 0) {
    //       res.forEach(element => {
    //         const airline = {
    //           airlineId: element.airlineId,
    //           city: element.city,
    //           state: element.state,
    //           name: element.name,
    //           logo: (element.logo === null) ? null : this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${element.logo}`),
    //           about: element.about,
    //           destinations: element.destinations
    //         };
    //         count++;
    //         if (count <= 5) {
    //           this.allAirlines.push(airline);
    //         }
    //       });
    //     }
    //   },
    //   err => {
    //     console.log(err);
    //   }
    // );
  }

  loadRentACarServices() {
    let count = 0;
    const a = this.rentService.getRACs().subscribe(
      (res: any[]) => {
        if (res.length > 0) {
          res.forEach(element => {
            const rac = {
              racId: element.id,
              city: element.city,
              state: element.state,
              name: element.name,
              logo: (element.logo === null) ? null : this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${element.logo}`),
              about: element.about,
              branches: element.branches
            };
            count++;
            if (count <= 5) {
              this.allRentACarServices.push(rac);
            }
          });
        }
      },
      err => {
        console.log(err);
      }
    );
  }

}
