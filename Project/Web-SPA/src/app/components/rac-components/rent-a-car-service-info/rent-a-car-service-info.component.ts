import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RentACarService } from 'src/app/entities/rent-a-car-service';
import { CarRentService } from 'src/services/car-rent.service';
import { Location } from '@angular/common';
import { DomSanitizer } from '@angular/platform-browser';


@Component({
  selector: 'app-rent-a-car-service-info',
  templateUrl: './rent-a-car-service-info.component.html',
  styleUrls: ['./rent-a-car-service-info.component.scss' , '../../al-components/airline-info/airline-info.component.scss']
})
export class RentACarServiceInfoComponent implements OnInit {

  id: number;
  rac: any;

  isOk = false;

  constructor(private route: ActivatedRoute, private carService: CarRentService, private location: Location, private san: DomSanitizer) {
    route.params.subscribe(params => { this.id = params.id; });
  }

  ngOnInit(): void {
    window.scroll(0, 0);
    const a = this.carService.getRACProfile(this.id).subscribe(
      (res: any) => {
        console.log(res);
        const destina = [];
        res.branches.forEach(element => {
          const des = {
            city: element.city,
            state: element.state
          };
          destina.push(des);
        });
        this.rac = {
          city: res.city,
          state: res.state,
          lat: res.lat,
          lon: res.lon,
          name: res.name,
          logo: (res.logo === null) ? null : this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${res.logo}`),
          about: res.about,
          branches: destina
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
