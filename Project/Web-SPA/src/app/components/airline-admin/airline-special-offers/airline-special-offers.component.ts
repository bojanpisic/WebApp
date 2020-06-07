import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AirlineService } from 'src/services/airline.service';
import { SpecialOfferService } from 'src/services/special-offer.service';
import { SpecialOffer } from 'src/app/entities/special-offer';

@Component({
  selector: 'app-airline-special-offers',
  templateUrl: './airline-special-offers.component.html',
  styleUrls: ['./airline-special-offers.component.scss']
})
export class AirlineSpecialOffersComponent implements OnInit {

  adminId: number;
  airlineId: number;

  searchText = '';

  specialOffers: Array<SpecialOffer>;

  constructor(private router: Router, private routes: ActivatedRoute, private airlineService: AirlineService,
              private specialOfferService: SpecialOfferService) {
    routes.params.subscribe(param => {
      this.adminId = param.id;
    });
  }

  ngOnInit(): void {
    this.airlineId = this.airlineService.getAdminsAirlineId(this.adminId);
    this.specialOffers = this.specialOfferService.getSpecialOffersOfSpecificAirline(this.airlineId);
  }

  goBack() {
    this.router.navigate(['/admin/' + this.adminId]);
  }

  focusInput() {
    document.getElementById('searchInput').focus();
  }

}
