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
    if (this.userId !== undefined) {
      // this.user = this.userService.getUser(this.userId);
    }
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
