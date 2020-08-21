import { Component, OnInit } from '@angular/core';
import {Chart} from 'chart.js';
import { Router, ActivatedRoute } from '@angular/router';
import { AirlineService } from 'src/services/airline.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-airline-stats',
  templateUrl: './airline-stats.component.html',
  styleUrls: ['./airline-stats.component.scss']
})
export class AirlineStatsComponent implements OnInit {

  myChart;
  adminId;
  types: Array<any>;
  dropdown = false;
  pickedType;
  pickedDate = '2020-08-30';
  pickedWeek = '2020-W35';
  pickedMonth = '2020-09';
  labels: Array<any>;
  dataset: Array<any>;

  constructor(private router: Router,
              private routes: ActivatedRoute,
              private airlineService: AirlineService,
              private toastr: ToastrService) {
    routes.params.subscribe(route => {
      this.adminId = route.id;
    });
    this.types = new Array<any>();
    this.types.push({
      type: 'date', displayedType: 'By date'
    });
    this.types.push({
      type: 'week', displayedType: 'By week'
    });
    this.types.push({
      type: 'month', displayedType: 'By month'
    });
    this.pickedType = this.types[0];
    this.labels = new Array<any>();
    this.dataset = new Array<any>();
  }

  ngOnInit(): void {
    this.onDateSelected();
    this.updateChart();
  }

  updateChart() {
    this.myChart = new Chart('myChart', {
      type: 'line',
      data: {
        labels: this.labels,
        datasets: [{
          data: this.dataset,
          label: this.pickedType.type === 'date' ? this.pickedDate :
                this.pickedType.type === 'week' ? this.pickedWeek :
                this.pickedMonth,
          borderColor: 'rgba(75, 192, 192, 1)',
          backgroundColor: 'rgba(75, 192, 192, 0.2)',
        }
        ]
      },
      options: {
        title: {
          display: true,
          text: this.pickedType.type === 'date' ? 'Income for  ' + this.pickedDate :
                this.pickedType.type === 'week' ? 'Income for  ' + this.pickedWeek :
                'Income for  ' + this.pickedMonth,
        }
      }
    });
  }

  onDateSelected() {
    const data = {
      date: this.pickedDate
    };
    const a = this.airlineService.getStatsForDate(data).subscribe(
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
    const a = this.airlineService.getStatsForWeek(data).subscribe(
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
    const a = this.airlineService.getStatsForMonth(data).subscribe(
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

  onExit() {
    this.router.navigate(['/admin/' + this.adminId]);
  }

  setType(value: any) {
    if (value.type === 'date') {
      this.onDateSelected();
    } else if (value.type === 'week') {
      this.onWeekSelected();
    } else {
      this.onMonthSelected();
    }
    this.pickedType = value;
    this.updateChart();
  }

  toggleDropDown() {
    this.dropdown = !this.dropdown;
  }
}
