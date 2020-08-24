import { Component, OnInit, Input } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { AirlineService } from 'src/services/airline.service';
import { RentACarService } from 'src/app/entities/rent-a-car-service';
import { CarRentService } from 'src/services/car-rent.service';
import { DomSanitizer } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-top-rated',
  templateUrl: './top-rated.component.html',
  styleUrls: ['./top-rated.component.scss']
})
export class TopRatedComponent implements OnInit {

  @Input() option: string;

  allRentACarServices: Array<any>;
  allAirlines: Array<any>;

  constructor(private airlineService: AirlineService,
              private rentService: CarRentService,
              private san: DomSanitizer,
              private toastr: ToastrService) {
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
    const a = this.airlineService.getTopRatedAirlines().subscribe(
      (res: any[]) => {
        if (res.length > 0) {
          console.log('TOP AIRLINES' + res);
          res.forEach(element => {
            console.log(element);
            const airline = {
              airlineId: element.airlineId,
              city: element.city,
              state: element.state,
              name: element.name,
              // tslint:disable-next-line:max-line-length
              logo: (element.logo === null) ? null : this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${element.logo}`),
              about: element.about,
              destinations: element.destinations,
              rate: element.rate
            };
            count++;
            if (count <= 5) {
              this.allAirlines.push(airline);
            }
          });
        }
      },
      err => {
        this.toastr.error(err.statusText, 'Error!');
      }
    );
  }

  loadRentACarServices() {
    let count = 0;
    const a = this.rentService.getTopRatedRACs().subscribe(
      (res: any[]) => {
        if (res.length > 0) {
          res.forEach(element => {
            const grade = element.item1;
            const rac = {
              racId: element.item2.id,
              city: element.item2.city,
              state: element.item2.state,
              name: element.item2.name,
              // tslint:disable-next-line:max-line-length
              logo: (element.item2.logo === null) ? null : this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${element.item2.logo}`),
              about: element.item2.about,
              branches: element.item2.branches,
            };
            count++;
            if (count <= 5) {
              this.allRentACarServices.push({
                rate: grade,
                racId: rac.racId,
                city: rac.city,
                state: rac.state,
                name: rac.name,
                logo: rac.logo,
                about: rac.about,
                branches: rac.branches
              });
            }
          });
        }
        console.log(res);
      },
      err => {
        this.toastr.error(err.statusText, 'Error!');
      }
    );
  }

}
