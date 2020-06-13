import { Component, OnInit, Input } from '@angular/core';
import { AirlineService } from 'src/services/airline.service';
import { Airline } from '../../../entities/airline';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Destination } from 'src/app/entities/destination';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-airline-info',
  templateUrl: './airline-info.component.html',
  styleUrls: ['./airline-info.component.scss']
})
export class AirlineInfoComponent implements OnInit {

  id: number;
  airline: any;

  isOk = false;

  // tslint:disable-next-line:max-line-length
  constructor(private route: ActivatedRoute, private san: DomSanitizer, private airlineService: AirlineService, private location: Location) {
    route.params.subscribe(params => { this.id = params.id; });
  }

  ngOnInit(): void {
    window.scroll(0, 0);
    const a = this.airlineService.getAirlineProfile(this.id).subscribe(
      (res: any) => {
        console.log(res);
        const destina = [];
        res.destinations.forEach(element => {
          const des = {
            imageUrl: this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${element.imageUrl}`),
            city: element.city,
            state: element.state
          };
          destina.push(des);
        });
        this.airline = {
          city: res.city,
          state: res.state,
          lat: res.lat,
          lon: res.lon,
          name: res.name,
          logo: (res.logo === null) ? null : this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${res.logo}`),
          about: res.about,
          destinations: destina
        };
        this.isOk = true;
      },
      err => {
        console.log(err);
      }
    );
  }

  goBack() {
    this.location.back();
  }
}
