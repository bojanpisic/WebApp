import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AirlineService } from 'src/services/airline.service';
import { FlightService } from 'src/services/flight.service';
import { Flight } from 'src/app/entities/flight';
import { Seat } from 'src/app/entities/seat';
import { SpecialOffer } from 'src/app/entities/special-offer';

@Component({
  selector: 'app-add-special-offer',
  templateUrl: './add-special-offer.component.html',
  styleUrls: ['./add-special-offer.component.scss']
})
export class AddSpecialOfferComponent implements OnInit {

  adminId: number;
  airlineId: number;

  searchText = '';

  specialOffer: SpecialOffer;

  flights: Array<Flight>;

  chooseSeat = false;
  choosenFlight: Flight;

  constructor(private router: Router, private routes: ActivatedRoute, private airlineService: AirlineService,
              private flightService: FlightService) {
    routes.params.subscribe(param => {
      this.adminId = param.id;
    });
  }

  ngOnInit(): void {
    this.airlineId = this.airlineService.getAdminsAirlineId(this.adminId);
    this.flights = this.flightService.getFlightsOfSpecificAirline(this.airlineId);
    this.specialOffer = new SpecialOffer();
    // this.addedFlights = this.flights;
  }

  goBack() {
    this.router.navigate(['/admin/' + this.adminId + '/special-offers']);
  }

  toggleChooseSeat(flight?: Flight) {
    console.log(flight);
    if (flight !== undefined) {
      this.choosenFlight = flight;
    }
    this.chooseSeat = !this.chooseSeat;
  }

  onAddSeat(value: any) {
    this.specialOffer.seats.push(value);
    this.specialOffer.flights.push(this.choosenFlight);
    this.chooseSeat = !this.chooseSeat;
  }

  onFinish() {
    // dodaj special offer
  }

}
