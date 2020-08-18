import { Component, OnInit, HostListener } from '@angular/core';
import { Airline } from '../../entities/airline';
import { RouterLinkActive, ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/entities/user';
import { UserService } from 'src/services/user.service';
// import {Chart} from 'chart.js';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  fly = true;
  drive = false;
  option = 'fly';
  userId: string;
  user: User;

  data: Airline;

  myChart;

  constructor(private route: ActivatedRoute, private userService: UserService, private router: Router) {
    route.params.subscribe(params => {
      this.userId = params.id;
    });
  }

  ngOnInit(): void {
    // if (this.userId !== undefined) {
    //   // this.user = this.userService.getUser(this.userId);
    // }
    // this.myChart = new Chart('myChart', {
    //     type: 'bar',
    //     data: {
    //         labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
    //         datasets: [{
    //             label: '# of Votes',
    //             data: [12, 19, 3, 5, 2, 3],
    //             backgroundColor: [
    //                 'rgba(255, 99, 132, 0.2)',
    //                 'rgba(54, 162, 235, 0.2)',
    //                 'rgba(255, 206, 86, 0.2)',
    //                 'rgba(75, 192, 192, 0.2)',
    //                 'rgba(153, 102, 255, 0.2)',
    //                 'rgba(255, 159, 64, 0.2)'
    //             ],
    //             borderColor: [
    //                 'rgba(255, 99, 132, 1)',
    //                 'rgba(54, 162, 235, 1)',
    //                 'rgba(255, 206, 86, 1)',
    //                 'rgba(75, 192, 192, 1)',
    //                 'rgba(153, 102, 255, 1)',
    //                 'rgba(255, 159, 64, 1)'
    //             ],
    //             borderWidth: 1
    //         }]
    //     },
    //     options: {
    //         scales: {
    //             yAxes: [{
    //                 ticks: {
    //                     beginAtZero: true
    //                 }
    //             }]
    //         }
    //     }
    // });
    // console.log(this.myChart);
  }

  onFly() {
    if (!this.fly) {
      this.fly = true;
      this.drive = false;
      this.option = 'fly';
    }
  }

  onDrive() {
    if (!this.drive) {
      this.fly = false;
      this.drive = true;
      this.option = 'drive';
    }
  }

  onSpecialOffers() {
    console.log('usa ba1');
    if (this.option === 'fly') {
      console.log('usa ba');
      this.router.navigate(['/flight-special-offers']);
    } else {
      console.log('usa ba2');
      this.router.navigate(['/car-special-offers']);
    }
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
      }

}
