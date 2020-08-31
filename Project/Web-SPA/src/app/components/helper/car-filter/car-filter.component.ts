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
  minSeatsNumber = 0;
  maxSeatsNumber = 10;

  airlinesButton: string;
  allCompanies: Array<any>;
  checkedCompanies: Array<boolean>;

  fromString: string;
  toString: string;
  depString: string;

  urlParams: any;
  userId: number;
  url: any;
  allCompaniesBool = false;
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
    this.allCompanies = new Array<any>();
    this.checkedCompanies = new Array<boolean>();
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
      racs: data.racs,
    };

    this.slidingMinPriceValue = (this.url.minPrice === 0) ? 0 : this.url.minPrice / 30;
    this.slidingMaxPriceValue = (this.url.maxPrice === 0) ? 0 : this.url.maxPrice / 30;
    this.minSeatsNumber = this.url.seatFrom;
    this.maxSeatsNumber = this.url.seatTo;
    this.allCompaniesBool = (this.url.racs === '') ? true : false;

    this.loadCars();
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

  allCompaniesButton() {
    this.checkedCompanies.forEach((v, i, a) => a[i] = true);
  }

  loadCars() {
    if (this.allCompaniesBool) {
      const a = this.racService.getRACs().subscribe(
        (res: any[]) => {
          if (res.length > 0) {
            res.forEach(element => {
              const airline = {
                id: element.id,
                name: element.name,
              };
              this.allCompanies.push(airline);
              this.checkedCompanies.push(true);
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
              this.allCompanies.push(airline);
              console.log(this.url.racs);
              const a111 = this.url.racs.split(',');
              console.log(airline.id);
              if (this.url.racs.split(',').includes(airline.id.toString())) {
                this.checkedCompanies.push(true);
              } else {
                this.checkedCompanies.push(false);
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

  toggleCompanyCheckBox(index: number) {
    this.checkedCompanies[index] = !this.checkedCompanies[index];
  }

  goBack() {
    this.closeFilter.emit(true);
  }

  onApplyFilters(value: any) {
    let racIds = '';
    const companies = this.getIdsOfCheckedCompanies();
    console.log(companies);
    for (let i = 0; i < companies.length; i++) {
      const element = companies[i];
      if (i === companies.length - 1) {
        racIds += element;
      } else {
        racIds += element + ',';
      }
    }

    const queryParams: any = {};
    const array = [];
    // if (this.url.type === 'one') {
    //   array.push({type: 'one'});
    //   array.push({from: this.url.from, to: this.url.to, dep: this.url.dep});
    // }
    // if (this.url.type === 'two') {
    //   array.push({type: 'two'});
    //   array.push({from: this.url.from, to: this.url.to,
    //               dep: this.url.dep, ret: this.url.ret});
    // }
    // if (this.url.type === 'multi') {
    //   array.push({type: 'multi'});
    //   this.fromString = this.url.from;
    //   this.toString = this.url.to;
    //   this.depString = this.url.dep;
    //   const fromSplitted = this.fromString.split(',');
    //   const toSplitted = this.toString.split(',');
    //   const depSplitted = this.depString.split(',');
    //   for (let i = 0; i < fromSplitted.length - 1; i++) {
    //     const element = fromSplitted[i];
    //     array.push({from: element, to: toSplitted[i], dep: depSplitted[i]});
    //   }
    // }

    array.push({
      type: '',
      from: this.url.from,
      to: this.url.to,
      dep: this.url.dep,
      ret: this.url.ret,
      minPrice: this.slidingMinPriceValue * 30,
      maxPrice: this.slidingMaxPriceValue * 30,
      racs: racIds,
      seatFrom: this.minSeatsNumber,
      seatTo: this.maxSeatsNumber
    });

    queryParams.array = JSON.stringify(array);

    const navigationExtras: NavigationExtras = {
      queryParams
    };
    if (this.userId !== undefined) {
      this.router.navigate(['/' + this.userId + '/cars'], navigationExtras);
    } else {
      this.router.navigate(['/cars'], navigationExtras);
    }

    this.appliedFilters.emit(true);
  }

  getIdsOfCheckedCompanies() {
    const ids = [];
    for (let i = 0; i < this.checkedCompanies.length; i++) {
      const element = this.checkedCompanies[i];
      if (element) {
        ids.push(this.allCompanies[i].id);
      }
    }
    return ids;
  }

}
