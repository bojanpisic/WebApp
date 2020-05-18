import { Component, OnInit, Input } from '@angular/core';
import { Destination } from 'src/app/entities/destination';
import { ActivatedRoute } from '@angular/router';
import { AirlineAdmin } from 'src/app/entities/airlineAdmin';
import { UserService } from 'src/services/user.service';
import { AirlineService } from 'src/services/airline.service';
import { Address } from 'src/app/entities/address';

@Component({
  selector: 'app-airline-destinations',
  templateUrl: './airline-destinations.component.html',
  styleUrls: ['./airline-destinations.component.scss']
})
export class AirlineDestinationsComponent implements OnInit {

  adminId: number;
  admin: AirlineAdmin;
  destinations: Array<Destination>;
  indexOfPickedDestination: number;
  pickedDestinationAddress: Address;
  showModal = false;

  searchText = '';

  constructor(private routes: ActivatedRoute, private userService: UserService, private airlineService: AirlineService) {
    routes.params.subscribe(route => {
      this.adminId = route.id;
    });
    this.destinations = new Array<Destination>();
  }

  ngOnInit(): void {
    this.admin = this.userService.getAirlineAdmin(this.adminId);
    this.destinations = this.airlineService.getDestinations(this.admin.airlineId);
  }

  onDelete(index: number) {
    this.indexOfPickedDestination = index;
    this.pickedDestinationAddress = this.destinations[index].address;
    this.showModal = true;
  }

  onDeleteDestination(value: boolean) {
    if (value) {
      this.destinations.splice(this.indexOfPickedDestination, 1);
    }
    this.showModal = false;
  }

  addDestination(value: any) {
    const obj = JSON.parse(value);
    const address = new Address(obj.city, obj.state, obj.short_name, obj.longitude, obj.latitude);
    const destination = new Destination(obj.placePhoto, address);
    // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < this.destinations.length; i++) {
      if (this.destinations[i].address.city === obj.city && this.destinations[i].address.state === obj.state) {
        return;
      }
    }
    this.destinations.push(destination);
  }

}
