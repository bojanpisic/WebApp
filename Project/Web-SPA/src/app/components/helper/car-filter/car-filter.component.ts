import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { NavigationExtras, Router, ActivatedRoute } from '@angular/router';
import { CarRentService } from 'src/services/car-rent.service';

@Component({
  selector: 'app-car-filter',
  templateUrl: './car-filter.component.html',
  styleUrls: ['./car-filter.component.scss']
})
export class CarFilterComponent implements OnInit {

  slidingMinPriceValue: number;
  slidingMaxPriceValue: number;
  minHoursDuration = 0;
  minMinutesDuration = 0;
  maxHoursDuration = 40;
  maxMinutesDuration = 0;

  airlinesButton: string;
  allAirlines: Array<any>;
  checkedAirlines: Array<boolean>;

  fromString: string;
  toString: string;
  depString: string;

  urlParams: any;
  userId: number;
  url: any;
  allAirlinesBool = false;
  @Output() closeFilter = new EventEmitter<boolean>();
  @Output() appliedFilters = new EventEmitter<any>();

  constructor(private racService: CarRentService,
              private router: Router, private route: ActivatedRoute) {
    const array = route.snapshot.queryParamMap.get('array');
    this.urlParams = JSON.parse(array);
    this.route.params.subscribe(param => {
      this.userId = param.id;
    });

    this.slidingMinPriceValue = 0;
    this.slidingMaxPriceValue = 100;
    this.allAirlines = new Array<any>();
    this.checkedAirlines = new Array<boolean>();
  }

  ngOnInit(): void {
    let data;
    data = this.generateFilter();

    console.log(this.urlParams);

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

    this.slidingMinPriceValue = (this.url.minPrice === 0) ? 0 : this.url.minPrice / 30;
    this.slidingMaxPriceValue = (this.url.maxPrice === 0) ? 0 : this.url.maxPrice / 30;
    this.minHoursDuration = this.url.seatFrom;
    this.maxHoursDuration = this.url.seatTo;
    this.allAirlinesBool = (this.url.racs === '') ? true : false;

    console.log(this.url);
    console.log(this.slidingMinPriceValue);
    console.log(this.slidingMaxPriceValue);
    console.log(this.minHoursDuration);
    console.log(this.minMinutesDuration);
    console.log(this.maxHoursDuration);
    console.log(this.maxMinutesDuration);

    // this.loadAirlines();
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
      return {type: this.urlParams[0].type, from: this.urlParams[0].from, to: this.urlParams[0].to,
        dep: this.urlParams[0].dep, ret: this.urlParams[0].ret, minPrice: this.urlParams[0].minPrice,
        maxPrice: this.urlParams[0].maxPrice,
        racs: this.urlParams[0].racs, seatFrom: this.urlParams[0].seatFrom, seatTo: this.urlParams[0].seatTo};
    }
  }

  getLeftStylePrice() {
    return this.slidingMinPriceValue + '%';
  }

  getRightStylePrice() {
    return (100 - +this.slidingMaxPriceValue).toString() + '%';
  }

  leftValuePrice() {
    return Math.min(+this.slidingMinPriceValue, +this.slidingMaxPriceValue) - 1;
  }

  rightValuePrice() {
    return Math.max(+this.slidingMinPriceValue, +this.slidingMaxPriceValue) + 1;
  }

  allAirlinesButton() {
    this.checkedAirlines.forEach((v, i, a) => a[i] = true);
  }

  loadCars() {
    if (this.allAirlinesBool) {
      const a = this.racService.getRACs().subscribe(
        (res: any[]) => {
          if (res.length > 0) {
            res.forEach(element => {
              const airline = {
                id: element.id,
                name: element.name,
              };
              this.allAirlines.push(airline);
              this.checkedAirlines.push(true);
              console.log(airline);
            });
          }
        },
        err => {
          console.log(err);
        }
      );
    } else {
      const a = this.racService.getRACs().subscribe(
        (res: any[]) => {
          if (res.length > 0) {
            res.forEach(element => {
              const airline = {
                id: element.id,
                name: element.name,
              };
              this.allAirlines.push(airline);
              if (this.url.air.split(',').contains(airline.id)) {
                this.checkedAirlines.push(true);
              } else {
                this.checkedAirlines.push(false);
              }
              console.log(airline);
            });
          }
        },
        err => {
          console.log(err);
        }
      );
    }
  }

  toggleAirlineCheckBox(index: number) {
    this.checkedAirlines[index] = !this.checkedAirlines[index];
  }

  goBack() {
    this.closeFilter.emit(true);
  }

  onApplyFilters() {
    let airIds = '';
    if (this.checkedAirlines.length !== this.allAirlines.length) {
      const airlines = this.getIdsOfCheckedAirlines();
      for (let i = 0; i < airlines.length; i++) {
        const element = airlines[i];
        if (i === airlines.length - 1) {
          airIds += element;
        } else {
          airIds += element + ',';
        }
      }
    }

    const queryParams: any = {};
    const array = [];
    if (this.url.type === 'one') {
      array.push({type: 'one'});
      array.push({from: this.url.from, to: this.url.to, dep: this.url.dep});
    }
    if (this.url.type === 'two') {
      array.push({type: 'two'});
      array.push({from: this.url.from, to: this.url.to,
                  dep: this.url.dep, ret: this.url.ret});
    }
    if (this.url.type === 'multi') {
      array.push({type: 'multi'});
      this.fromString = this.url.from;
      this.toString = this.url.to;
      this.depString = this.url.dep;
      const fromSplitted = this.fromString.split(',');
      const toSplitted = this.toString.split(',');
      const depSplitted = this.depString.split(',');
      for (let i = 0; i < fromSplitted.length - 1; i++) {
        const element = fromSplitted[i];
        array.push({from: element, to: toSplitted[i], dep: depSplitted[i]});
      }
    }

    array.push({
      minPrice: this.slidingMinPriceValue * 30,
      maxPrice: this.slidingMaxPriceValue * 30,
      mind: this.minHoursDuration + 'h ' + this.minMinutesDuration + 'min',
      maxd: this.maxHoursDuration + 'h ' + this.maxMinutesDuration + 'min',
      air: airIds
    });

    queryParams.array = JSON.stringify(array);

    const navigationExtras: NavigationExtras = {
      queryParams
    };
    if (this.userId !== undefined) {
      this.router.navigate(['/' + this.userId + '/trips'], navigationExtras);
    } else {
      this.router.navigate(['/trips'], navigationExtras);
    }
  }

  getIdsOfCheckedAirlines() {
    const ids = [];
    for (let i = 0; i < this.checkedAirlines.length; i++) {
      const element = this.checkedAirlines[i];
      if (element) {
        ids.push(this.allAirlines[i].airlineId);
      }
    }
    return ids;
  }

}
