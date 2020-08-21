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
    const a = this.airlineService.getAirlines().subscribe(
      (res: any[]) => {
        if (res.length > 0) {
          res.forEach(element => {
            console.log(element);
            // {
            //   pretpostavljam da cu dobiti:
            //   {
            //     [4.5, {airlineId: 5, city: 'Nzm', ...}],
            //     [4.2, {airlineId: 5, city: 'Nzm', ...}],
            //     [3.4, {airlineId: 5, city: 'Nzm', ...}],
            //     ...
            //   }
            // }
            // ako ne kopiraj mi console log i nalepi ovde da ispravim
            const grade = element[0];
            const airline = {
              airlineId: element[1].airlineId,
              city: element[1].city,
              state: element[1].state,
              name: element[1].name,
              // tslint:disable-next-line:max-line-length
              logo: (element[1].logo === null) ? null : this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${element[1].logo}`),
              about: element[1].about,
              destinations: element[1].destinations
            };
            count++;
            if (count <= 5) {
              this.allAirlines.push({
                rate: grade,
                airlineId: airline.airlineId,
                city: airline.city,
                state: airline.state,
                name: airline.name,
                logo: airline.logo,
                about: airline.about,
                destinations: airline.destinations
              });
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
    const a = this.rentService.getRACs().subscribe(
      (res: any[]) => {
        if (res.length > 0) {
          res.forEach(element => {
            const grade = element[0];
            const rac = {
              racId: element[1].id,
              city: element[1].city,
              state: element[1].state,
              name: element[1].name,
              // tslint:disable-next-line:max-line-length
              logo: (element[1].logo === null) ? null : this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${element[1].logo}`),
              about: element[1].about,
              branches: element[1].branches
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
      },
      err => {
        this.toastr.error(err.statusText, 'Error!');
      }
    );
  }

}
