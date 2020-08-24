import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CarRentService } from 'src/services/car-rent.service';
import {Chart} from 'chart.js';

@Component({
  selector: 'app-rac-stats',
  templateUrl: './rac-stats.component.html',
  styleUrls: ['./rac-stats.component.scss']
})
export class RacStatsComponent implements OnInit {

  adminId;
  types: Array<any>;
  chartTypes: Array<any>;
  dropdown = false;
  dropdownChart = false;
  pickedType;
  pickedTypeChart;
  from = '2020-08-30';
  to = '2020-10-30';
  pickedDate = '2020-08-30';
  pickedWeek = '2020-W35';
  pickedMonth = '2020-09';
  cars: Array<any>;
  labels; dataset; myChart;

  constructor(private router: Router,
              private routes: ActivatedRoute,
              private carService: CarRentService,
              private toastr: ToastrService) {
    routes.params.subscribe(route => {
      this.adminId = route.id;
    });
    this.cars = new Array<any>();
    this.types = new Array<any>();
    this.chartTypes = new Array<any>();
    this.types.push({
      type: 'free', displayedType: 'Free vehicles'
    });
    this.types.push({
      type: 'rented', displayedType: 'Rented vehicles'
    });
    this.pickedType = this.types[0];
    this.chartTypes.push({
      type: 'date', displayedType: 'By date'
    });
    this.chartTypes.push({
      type: 'week', displayedType: 'By week'
    });
    this.chartTypes.push({
      type: 'month', displayedType: 'By month'
    });
    this.pickedTypeChart = this.chartTypes[0];
    this.labels = new Array<any>();
    this.dataset = new Array<any>();
  }

  ngOnInit(): void {
    this.getValues();
    this.updateChart();
  }

  updateChart() {
    this.myChart = new Chart('myChart', {
      type: 'line',
      data: {
        labels: this.labels,
        datasets: [{
          data: this.dataset,
          label: this.pickedTypeChart.type === 'date' ? this.pickedDate :
                this.pickedTypeChart.type === 'week' ? this.pickedWeek :
                this.pickedMonth,
          borderColor: 'rgba(75, 192, 192, 1)',
          backgroundColor: 'rgba(75, 192, 192, 0.2)',
        }
        ]
      },
      options: {
        title: {
          display: true,
          text: this.pickedTypeChart.type === 'date' ? 'Income for  ' + this.pickedDate :
                this.pickedTypeChart.type === 'week' ? 'Income for  ' + this.pickedWeek :
                'Income for  ' + this.pickedMonth,
        }
      }
    });
  }

  onDateSelected() {
    const data = {
      date: this.pickedDate
    };
    const a = this.carService.getStatsForDate(data).subscribe(
      (res: any) => {
        this.labels.push(this.pickedDate);
        this.dataset.push(res.result);
      },
      err => {
        this.toastr.error(err.statusText, 'Error.');
      }
    );
  }

  onWeekSelected() {
    const data = {
      week: this.pickedWeek
    };
    const a = this.carService.getStatsForWeek(data).subscribe(
      (res: any) => {
        // ovde mi moras vratiti dane te nedelje tipa:
        // [2020-05-20, 2020-05-21, 2020-05-22, 2020-05-23...]
        // nek bude u days
        // tslint:disable-next-line:forin
        for (const key in res.days) {
          this.labels.push(key);
        }
        // a onda mi u tickets vrati niz prodatih karti za te dane tih 7
        // tipa [200, 245, 233, 432, ...]
        // tslint:disable-next-line:forin
        for (const key in res.tickets) {
          this.dataset.push(key);
        }
      },
      err => {
        this.toastr.error(err.statusText, 'Error.');
      }
    );
  }

  onMonthSelected() {
    const data = {
      month: this.pickedMonth
    };
    const a = this.carService.getStatsForMonth(data).subscribe(
      (res: any) => {
        // isto ko i sa week samo ovde nema 7 vec 28,29,30 ili 31 dan
        // tslint:disable-next-line:forin
        for (const key in res.days) {
          this.labels.push(key);
        }
        // isto ko i sa week samo ovde nema 7 vec 28,29,30 ili 31 dan
        // tslint:disable-next-line:forin
        for (const key in res.tickets) {
          this.dataset.push(key);
        }
      },
      err => {
        this.toastr.error(err.statusText, 'Error.');
      }
    );
  }

  setChartType(value: any) {
    if (value.type === 'date') {
      this.onDateSelected();
    } else if (value.type === 'week') {
      this.onWeekSelected();
    } else {
      this.onMonthSelected();
    }
    this.pickedTypeChart = value;
    this.updateChart();
  }

  toggleChartDropDown() {
    this.dropdownChart = !this.dropdownChart;
  }

  getValues() {
    const data = {
      from: this.from,
      to: this.to,
      isFree: (this.pickedType.type === 'free' ? true : false)
    };
    console.log(data);
    const a = this.carService.getStats(data).subscribe(
      (res: any) => {
        console.log(res);
        if (res.length > 0) {
          res.forEach(el => {
            const r = {
              brand: el.brand,
              carId: el.carId,
              city: el.city,
              model: el.model,
              name: el.name,
              pricePerDay: el.pricePerDay,
              seatsNumber: el.seatsNumber,
              state: el.state,
              type: el.type,
              year: el.year,
              rate: el.rate
            };
            this.cars.push(r);
          });
      }
    },
    err => {
      this.toastr.error(err.statusText, 'Error.');
    });
  }

  onExit() {
    this.router.navigate(['/admin/' + this.adminId]);
  }

  setType(value: any) {
    this.pickedType = value;
    this.getValues();
  }

  toggleDropDown() {
    this.dropdown = !this.dropdown;
  }

  onSearch() {
    this.cars = [];
    this.getValues();
  }
}
