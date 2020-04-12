import { Component, OnInit, HostListener } from '@angular/core';
import { Airline } from '../../entities/airline';
import { RouterLinkActive, ActivatedRoute } from '@angular/router';
import { User } from 'src/app/entities/user';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  fly = true;
  drive = false;
  option = 'fly';
  userId: number;
  user: User;

  data: Airline;

  constructor(private route: ActivatedRoute, private userService: UserService) {
    route.params.subscribe(params => {
      this.userId = params.id;
    });
  }

  ngOnInit(): void {
    if (this.userId !== undefined) {
      this.user = this.userService.getUser(this.userId);
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

  @HostListener('window:scroll', [])
  onWindowScroll() {
      }

}
