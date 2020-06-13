import { Component, OnInit } from '@angular/core';
import { RegisteredUser } from 'src/app/entities/registeredUser';
import { UserService } from 'src/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { Message } from 'src/app/entities/message';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {

  userId: number;
  user: RegisteredUser;
  myProps = {message: undefined, show: false};

  activeButton = 'inbox';

  constructor(private route: ActivatedRoute, private userService: UserService) {
    route.params.subscribe(params => {
      this.userId = params.id;
    });
   }

  ngOnInit(): void {
    const air1 = this.userService.getUser(this.userId);
    // const airline = this.airlineService.getAdminsAirline(this.adminId);
    // this.companyFields = {
    //   name: airline.name,
    //   location: airline.address,
    //   about: airline.about
    // };
    console.log(air1);
  }

  toggleButton(value: string) {
    this.activeButton = value;
  }

  getResponse(value: string) {
    this.myProps.show = false;
  }

  openMessageInfo(message: Message) {
    this.myProps.message = message;
    this.myProps.show = true;
  }

}
