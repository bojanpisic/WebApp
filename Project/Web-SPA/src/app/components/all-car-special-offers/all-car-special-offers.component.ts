import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CarRentService } from 'src/services/car-rent.service';
import { DomSanitizer } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-all-car-special-offers',
  templateUrl: './all-car-special-offers.component.html',
  styleUrls: ['./all-car-special-offers.component.scss']
})
export class AllCarSpecialOffersComponent implements OnInit {

  racId: number;
  userId: number;

  specialOffers: Array<{
    name: string,
    brand: string,
    model: string,
    year: number,
    type: string,
    newPrice: number,
    oldPrice: number,
    fromDate: string,
    toDate: string,
    seatsNumber: number
  }>;
  itsOk = false;

  constructor(private router: Router, private routes: ActivatedRoute, private carService: CarRentService
            , private san: DomSanitizer, private toastr: ToastrService) {
    routes.params.subscribe(param => {
      this.racId = param.id;
      this.userId = param.user;
    });
    this.specialOffers = [];
  }

  ngOnInit(): void {
    window.scroll(0, 0);
    if (this.racId === undefined) {
      this.carService.getAllSpecialOffers().subscribe(
        (res: any[]) => {
          if (res.length) {
            res.forEach(element => {
              const new1 = {
                name: element.name,
                brand: element.brand,
                model: element.model,
                year: element.year,
                type: element.type,
                newPrice: element.newPrice,
                oldPrice: element.oldPrice,
                fromDate: element.fromDate.split('T')[0],
                toDate: element.toDate.split('T')[0],
                seatsNumber: element.seatsNumber
              };
              this.specialOffers.push(new1);
            });
            this.itsOk = true;
          }
          console.log(res);
        },
        err => {
          this.toastr.error(err.statusText, 'Error!');
        }
      );
    } else {
      this.carService.getRACSpecialOffers(this.racId).subscribe(
        (res: any[]) => {
          if (res.length) {
            res.forEach(element => {
              const new1 = {
                name: element.name,
                brand: element.brand,
                model: element.model,
                year: element.year,
                type: element.type,
                newPrice: element.newPrice,
                oldPrice: element.oldPrice,
                fromDate: element.fromDate.split('T')[0],
                toDate: element.toDate.split('T')[0],
                seatsNumber: element.seatsNumber
              };
              this.specialOffers.push(new1);
            });
            this.itsOk = true;
          }
          console.log(res);
        },
        err => {
          this.toastr.error(err.statusText, 'Error!');
        }
      );
    }
  }

  goBack() {
    if (this.userId !== undefined) {
      if (this.racId !== undefined) {
        this.router.navigate(['/' + this.userId + '/rent-a-car-services/' + this.racId + '/rent-a-car-service-info']);
      } else {
        this.router.navigate(['/' + this.userId + '/home']);
      }
    } else {
      if (this.racId !== undefined) {
        this.router.navigate(['/rent-a-car-services/' + this.racId + '/rent-a-car-service-info']);
      } else {
        this.router.navigate(['/']);
      }
    }
  }

}
