import { Component, OnInit } from '@angular/core';
import { Trip } from 'src/app/entities/trip';
import { TripService } from 'src/services/trip.service';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { RegisteredUser } from 'src/app/entities/registeredUser';
import { UserService } from 'src/services/user.service';
import { AirlineService } from 'src/services/airline.service';

@Component({
  selector: 'app-trips',
  templateUrl: './trips.component.html',
  styleUrls: ['./trips.component.scss']
})
export class TripsComponent implements OnInit {
  user: RegisteredUser;
  userId: number;
  trips: Array<any>;
  urlParams = [];
  filter = false;

  url: any;

  constructor(private userService: UserService, private tripService: TripService,
              private location: Location, private route: ActivatedRoute, private airlineService: AirlineService,
              private router: Router) {
    const array = route.snapshot.queryParamMap.get('array');
    this.urlParams = JSON.parse(array);
    console.log(this.urlParams);

    this.route.params.subscribe(param => {
      this.userId = param.id;
    });
    this.trips = new Array<any>();
   }

  ngOnInit(): void {
    let data;
    data = this.generateFilter();

    if (this.urlParams !== null) {
      this.url = {
        type: data.type,
        from: data.from,
        to: data.to,
        dep: data.dep,
        ret: data.ret,
        minPrice: data.minPrice,
        maxPrice: data.maxPrice,
        air: data.air,
        mind: data.mind,
        maxd: data.maxd
      };

      const a = this.airlineService.test(this.url).subscribe(
        (res: any[]) => {
          if (res.length > 0) {
            res.forEach(element => {
              // const airline = {
              //   minPrice: element.minPrice,
              //   name: element.name,
              // };
              // this.allAirlines.push(airline);
              // if (this.url.air.split(',').contains(airline.airlineId)) {
              //   this.checkedAirlines.push(true);
              // } else {
              //   this.checkedAirlines.push(false);
              // }
              // console.log(airline);
            });
          }
        },
        err => {
          console.log(err);
        }
      );
    }
  }

  generateFilter() {
    if (this.urlParams === null && this.userId === undefined) {
      this.router.navigate(['']);
      return;
    }
    if (this.urlParams === null && this.userId !== undefined) {
      this.router.navigate(['/' + this.userId + '/home']);
      return;
    }
    if (this.urlParams !== null) {
      if (this.urlParams[0].type === 'one') {
        return {type: 'one', from: this.urlParams[1].from, to: this.urlParams[1].to,
                dep: this.urlParams[1].dep, ret: '', minPrice: this.urlParams[2].minPrice, maxPrice: this.urlParams[2].maxPrice,
                air: this.urlParams[2].air, mind: this.urlParams[2].mind, maxd: this.urlParams[2].maxd};
      } else if (this.urlParams[0].type === 'two') {
        // tslint:disable-next-line:max-line-length
        return {type: 'one', from: this.urlParams[1].from, to: this.urlParams[1].to,
                dep: this.urlParams[1].dep, ret: this.urlParams[1].ret,
                minPrice: this.urlParams[2].minPrice, maxPrice: this.urlParams[2].maxPrice,
                air: this.urlParams[2].air, mind: this.urlParams[2].mind, maxd: this.urlParams[2].maxd};
      } else {
        let froms = '';
        let tos = '';
        let deps = '';
        for (let i = 1; i < this.urlParams.length; i++) {
          if (i === this.urlParams.length - 2) {
            const element = this.urlParams[i];
            froms += this.urlParams[i].from;
            tos += this.urlParams[i].to;
            deps += this.urlParams[i].dep;
          } else {
            const element = this.urlParams[i];
            froms += this.urlParams[i].from + ',';
            tos += this.urlParams[i].to + ',';
            deps += this.urlParams[i].dep + ',';
          }
        }
        return {type: 'multi', from: froms, to: tos, dep: deps, ret: '',
                minPrice: this.urlParams[this.urlParams.length - 1].minPrice,
                maxPrice: this.urlParams[this.urlParams.length - 1].maxPrice,
                air: this.urlParams[this.urlParams.length - 1].air,
                mind: this.urlParams[this.urlParams.length - 1].mind,
                maxd: this.urlParams[this.urlParams.length - 1].maxd};
      }
    }
  }

  onApplyFilter() {

  }

  toggleFilter() {
    this.filter = !this.filter;
  }

  goBack() {
    this.location.back();
  }

}
