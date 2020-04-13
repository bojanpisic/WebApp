import { Component, OnInit } from '@angular/core';
import { RegisteredUser } from 'src/app/entities/registeredUser';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-show-flights',
  templateUrl: './show-flights.component.html',
  styleUrls: ['./show-flights.component.scss']
})
export class ShowFlightsComponent implements OnInit {

  user: RegisteredUser;
  userId: number;

  constructor(private route: ActivatedRoute, private userService: UserService) {
    route.params.subscribe(params => {
      this.userId = params.id;
    });
  }

  ngOnInit(): void {
    this.user = this.userService.getUser(this.userId);
    console.log(this.user.flights);
  }

}
