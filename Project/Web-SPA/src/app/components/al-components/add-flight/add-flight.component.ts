import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AirlineService } from 'src/services/airline.service';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/entities/user';
import { UserService } from 'src/services/user.service';
import { Airline } from 'src/app/entities/airline';
import { Flight } from 'src/app/entities/flight';

@Component({
  selector: 'app-add-flight',
  templateUrl: './add-flight.component.html',
  styleUrls: ['./add-flight.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AddFlightComponent implements OnInit {

  constructor(private route: ActivatedRoute, private userService: UserService, private airlineService: AirlineService) {
    route.params.subscribe(params => {
      this.adminId = params.id; this.adminId = params.id; });
   }

  adminId: number;
  admin: User;
  adminsAirline: Airline;
  changeOverValue = '';
  changeoverlist: Array<string>;
  clicked: string;
  choosenChangeOvers: Array<string>;
  itemCopy: Array<string>;

  inputStyle = 'margin-top: 10px; \
                margin-right: 3px; \
                width: 20%; height: 60%; \
                display: inline; \
                background-color: rgb(81, 5, 221); \
                color: white; \
                border-radius: 10px; \
                overlay:none; \
                border: none; \
                font-size: 12px; \
                text-align: center; \
                padding: 1px;';

  ngOnInit(): void {
    this.userService.getAllUsers().forEach(user => {
      if (user.id === this.adminId) {
        this.admin = user;
        return;
      }
    });

    this.airlineService.allMockedAirlines().forEach(airline => {
      if (airline.adminid === this.admin.id) {
        this.adminsAirline = airline;
      }
    });

    this.changeoverlist = new Array<string>();
    this.choosenChangeOvers = new Array<string>();

    this.changeoverlist.push('London');
    this.changeoverlist.push('Madrid');
    this.changeoverlist.push('Rome');
  }

  addFlight() {
    //this.adminsAirline.flights.push(new Flight());
  }

  changeoverClicked(changeover: string) {
    this.changeOverValue = '';

    if (Number((document.getElementById('changeovernum') as HTMLInputElement).value) > this.choosenChangeOvers.length) {
      this.choosenChangeOvers.push(changeover);
      (document.getElementById('changeover') as HTMLInputElement).value = '';

      let child = document.createElement('input');
      child.setAttribute('value', changeover);
      child.setAttribute('id', changeover);
      child.setAttribute('type', 'button');
     // child.setAttribute('(click)', 'deleteChangeover(' + changeover + ')');
      child.addEventListener('click', () => {
        document.getElementById(child.getAttribute('id')).remove();
        this.choosenChangeOvers.splice(this.choosenChangeOvers.indexOf(child.getAttribute('id')), 1);
    });
      document.getElementById('changeover-list').appendChild(child);
    }
  }

  deleteChangeover(changeover: string) {
    document.getElementById(changeover).remove();
  }

  searchFunc() {
    this.changeOverValue = (document.getElementById('changeover') as HTMLInputElement).value;
  }
}
