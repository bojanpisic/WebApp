import { Component, OnInit } from '@angular/core';
import { SpecialOfferService } from 'src/services/special-offer.service';
import { ActivatedRoute } from '@angular/router';
import { SpecialOffer } from 'src/app/entities/special-offer';
import { AirlineService } from 'src/services/airline.service';
import { Airline } from 'src/app/entities/airline';

@Component({
  selector: 'app-special-offers',
  templateUrl: './special-offers.component.html',
  styleUrls: ['./special-offers.component.scss']
})
export class SpecialOffersComponent implements OnInit {

  option = 'oneWay';
  showInfo: Array<boolean>;
  airlineId: number;
  airline: Airline;
  specialOffers: Array<SpecialOffer>;
  anotherOneClicked = false;

  constructor(private route: ActivatedRoute, private specOfferService: SpecialOfferService, private airlineService: AirlineService) {
    route.params.subscribe(params => {
      this.airlineId = params.id;
    });
    this.specialOffers = new Array<SpecialOffer>();
    this.showInfo = new Array<boolean>();
   }

  ngOnInit(): void {
    this.specialOffers = this.specOfferService.getSpecialOffersOfSpecificAirline(this.airlineId);
    this.getAirline();
    this.showInfo.push(false);
  }
  getAirline() {
    this.airline = this.airlineService.getAirline(this.airlineId);
  }

  showStopsInfo(i: number) {
    this.showInfo[i] = !this.showInfo[i];
  }

}
