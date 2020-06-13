import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CarRentService } from 'src/services/car-rent.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-rac-special-offers',
  templateUrl: './rac-special-offers.component.html',
  styleUrls: ['./rac-special-offers.component.scss']
})
export class RacSpecialOffersComponent implements OnInit {

  adminId: number;
  airlineId: number;

  searchText = '';
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

  constructor(private router: Router, private routes: ActivatedRoute, private carService: CarRentService, private san: DomSanitizer) {
    routes.params.subscribe(param => {
      this.adminId = param.id;
    });
    this.specialOffers = [];
  }

  ngOnInit(): void {
    this.carService.getAdminsSpecialOffers().subscribe(
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
        console.log('dada' + err.status);
        // tslint:disable-next-line: triple-equals
        if (err.status == 400) {
          console.log('400' + err);
          // this.toastr.error('Incorrect username or password.', 'Authentication failed.');
        } else if (err.status === 401) {
          console.log(err);
        } else {
          console.log(err);
        }
      }
    );
  }

  goBack() {
    this.router.navigate(['/rac-admin/' + this.adminId]);
  }

  focusInput() {
    document.getElementById('searchInput').focus();
  }

}
