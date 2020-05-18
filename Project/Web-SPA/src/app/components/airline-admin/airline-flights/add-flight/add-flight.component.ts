// tslint:disable-next-line:max-line-length
import { Component, OnInit, ViewEncapsulation, ComponentFactoryResolver, ViewContainerRef, ViewChild, ComponentRef, ChangeDetectorRef, TemplateRef, AfterViewInit, AfterContentChecked, AfterContentInit } from '@angular/core';
import { AirlineService } from 'src/services/airline.service';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/entities/user';
import { UserService } from 'src/services/user.service';
import { Airline } from 'src/app/entities/airline';
import { Flight } from 'src/app/entities/flight';
import { Address } from 'src/app/entities/address';
import { FlightStopComponent } from './flight-stop/flight-stop.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Seat } from 'src/app/entities/seat';
import { Location } from '@angular/common';

@Component({
  selector: 'app-add-flight',
  templateUrl: './add-flight.component.html',
  styleUrls: ['./add-flight.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AddFlightComponent implements OnInit {

  flight: Flight;

  step = 1;
  stepOneConfirmed = false;
  stepTwoConfirmed = false;
  stepThreeConfirmed = false;
  stepFourConfirmed = false;

  adminId: number;
  admin: User;

  flightType = 'direct flight';

  formFrom: FormGroup;
  fromLocation: Address;
  lastGoodLocationString: string;
  lastLocationString: string;
  errorFromLocation = false;
  departureDateInvalid = false;
  departureTimeInvalid = false;

  stops: Array<Address>;
  currentStop: Address;
  isCurrentStopValid: boolean;
  sameLocationPicked = false;

  formTo: FormGroup;
  toLocation: Address;
  errorToLocation = false;
  arrivalTimeInvalid = false;
  arrivalDateInvalid = false;
  flightLengthInvalid = false;

  firstClassSeatsNumber = 10;
  firstClassSeatPrice = 200;
  businessSeatsNumber = 10;
  businessSeatPrice = 150;
  economySeatsNumber = 10;
  economySeatPrice = 100;
  basicEconomySeatsNumber = 10;
  basicEconomySeatPrice = 50;

  minusFirstClassDisabled = false;
  minusBusinessDisabled = false;
  minusEconomyDisabled = false;
  minusBasicEconomyDisabled = false;

  componentsReferences = [];
  @ViewChild('viewContainerRef', { read: ViewContainerRef }) VCR: ViewContainerRef;
  @ViewChild('viewContainerRef1', { read: ViewContainerRef }) VCR1: ViewContainerRef;
  @ViewChild('tpl', {read: TemplateRef}) tpl: TemplateRef<any>;

  constructor(private route: ActivatedRoute, private router: Router,
              private userService: UserService, private airlineService: AirlineService,
              private CFR: ComponentFactoryResolver, private location: Location) {
    route.params.subscribe(params => {
      this.adminId = params.id;
    });
    this.stops = new Array<Address>();
    this.flight = new Flight();
   }

  ngOnInit(): void {
    this.flight.airlineId = this.airlineService.getAirlineId(this.adminId);
    this.initFormFrom();
    this.initFormTo();
  }

  initFormFrom() {
    this.formFrom = new FormGroup({
      date: new FormControl('', Validators.required),
      time: new FormControl('', Validators.required),
   });
  }

  initFormTo() {
    this.formTo = new FormGroup({
      time: new FormControl('', Validators.required),
      date: new FormControl('', Validators.required),
      flightLength: new FormControl('', Validators.required),
   });
  }

  addFlight() {
    this.flight.flightNumber = 'TA872';
    this.flight.tripTime = '10';
    this.flight.ticketPrice = Math.min(this.firstClassSeatPrice, this.businessSeatPrice, this.economySeatPrice, this.basicEconomySeatPrice);
    this.exit();
  }

  goBack() {
    this.router.navigate(['/admin/' + this.adminId + '/flights']);
  }

  // FROM

  onFrom(value: any) {
    const obj = JSON.parse(value);
    this.fromLocation = new Address(obj.city, obj.state, obj.short_name, obj.longitude, obj.latitude);
    this.lastGoodLocationString = this.lastLocationString;
  }

  onFromInputChange(value: any) {
    this.lastLocationString = value;
  }

  onSubmitFrom() {
    if (this.validateFromForm()) {
      this.flight.from = this.fromLocation;
      this.flight.takeOffDate = this.formFrom.controls.date.value;
      this.flight.takeOffTime = this.formFrom.controls.time.value;
      this.nextStep();
    }
  }

  validateFromForm() {
    let retVal = true;
    if (this.fromLocation === undefined || this.lastGoodLocationString !== this.lastLocationString) {
      this.errorFromLocation = true;
      retVal = false;
    }
    if (this.formFrom.controls.date.value === '') {
      this.departureDateInvalid = true;
      retVal = false;
    }
    if (this.formFrom.controls.time.value === '') {
      this.departureTimeInvalid = true;
      retVal = false;
    }
    return retVal;
  }

  // TO

  onTo(value: any) {
    const obj = JSON.parse(value);
    this.toLocation = new Address(obj.city, obj.state, obj.short_name, obj.longitude, obj.latitude);
    this.lastGoodLocationString = this.lastLocationString;
  }

  onToInputChange(value: any) {
    this.lastLocationString = value;
  }

  onSubmitTo() {
    if (this.validateToForm()) {
      this.flight.to = this.toLocation;
      this.flight.landingDate = this.formTo.controls.date.value;
      this.flight.landingTime = this.formTo.controls.time.value;
      this.flight.tripLength = this.formTo.controls.flightLength.value;
      this.nextStep();
    }
  }

  validateToForm() {
    let retVal = true;
    if (this.toLocation === undefined || this.lastGoodLocationString !== this.lastLocationString) {
      this.errorFromLocation = true;
      retVal = false;
    }
    if (this.formTo.controls.date.value === '') {
      this.arrivalDateInvalid = true;
      retVal = false;
    }
    if (this.formTo.controls.time.value === '') {
      this.arrivalTimeInvalid = true;
      retVal = false;
    }
    if (this.formTo.controls.flightLength.value === '') {
      this.flightLengthInvalid = true;
      retVal = false;
    }
    return retVal;
  }

  // STOPS


  onFlightType(event: any) {
    this.flightType = event.target.value;
    if (this.flightType === 'with stops') {
      setTimeout(() => {
        this.clearView();
        this.createComponent();
      }, 100);
    }
  }

  addToTheView(value: Address) {
    if (this.VCR1 !== undefined) {
      this.VCR1.createEmbeddedView(this.tpl, {city: value.city, state: value.state, number: this.stops.length});
    }
  }

  clearView() {
    this.stops = [];
    this.VCR1.clear();
  }

  createComponent() {
    if (this.VCR !== undefined) {
      this.VCR.clear();
    }
    const componentFactory = this.CFR.resolveComponentFactory(FlightStopComponent);
    const componentRef: ComponentRef<FlightStopComponent> = this.VCR.createComponent(componentFactory);
    // const currentComponent = componentRef.instance;
    componentRef.instance.stop.subscribe($event => {
      this.addStop($event);
    });
    componentRef.instance.valid.subscribe($event => {
      this.isValidStop($event);
    });
  }

  isValidStop(value: boolean) {
    this.isCurrentStopValid = value;
  }

  addStop(value: Address) {
    this.currentStop = value;
  }

  onAddStop() {
    if (this.validateStop()) {
      let push = true;
      this.stops.forEach(stop => {
        if (stop.city === this.currentStop.city && stop.state === this.currentStop.state) {
          this.sameLocationPicked = true;
          push = false;
        }
      });
      if (push) {
        this.sameLocationPicked = false;
        // tslint:disable-next-line:max-line-length
        this.stops.push(new Address(this.currentStop.city, this.currentStop.state, this.currentStop.shortName, this.currentStop.lon, this.currentStop.lat));
        this.addToTheView(this.currentStop);
        this.currentStop = undefined;
        this.isCurrentStopValid = false;
        this.createComponent();
      }
    }
  }

  onRemoveStop() {

    if (this.VCR1.length < 1) {
            return;
    }

    this.stops.pop();

    this.VCR1.remove(this.VCR1.length - 1);
  }

  validateStop() {
    let retVal = true;
    if (this.currentStop === undefined) {
      retVal = false;
    }
    if (!this.isCurrentStopValid) {
      retVal = false;
    }
    return retVal;
  }

  loadPickedStops() {
    setTimeout(() => {
      if (this.stops.length === 0) {
        this.clearView();
      } else {
        this.stops.forEach(stop => {
          this.addToTheView(stop);
        });
      }
      this.createComponent();
    }, 100);
  }

  // SEATS

  onPlusFirstClass() {
    this.firstClassSeatsNumber++;
    this.minusFirstClassDisabled = false;
  }

  onMinusFirstClass() {
    if (this.firstClassSeatsNumber > 1) {
      this.firstClassSeatsNumber--;
    }
    if (this.firstClassSeatsNumber === 1) {
      this.minusFirstClassDisabled = true;
    }
  }

  onPlusBusiness() {
    this.businessSeatsNumber++;
    this.minusBusinessDisabled = false;
  }

  onMinusBusiness() {
    if (this.businessSeatsNumber > 1) {
      this.businessSeatsNumber--;
    }
    if (this.businessSeatsNumber === 1) {
      this.minusBusinessDisabled = true;
    }
  }

  onPlusEconomy() {
    this.economySeatsNumber++;
    this.minusEconomyDisabled = false;
  }

  onMinusEconomy() {
    if (this.economySeatsNumber > 1) {
      this.economySeatsNumber--;
    }
    if (this.economySeatsNumber === 1) {
      this.minusEconomyDisabled = true;
    }
  }

  onPlusBasicEconomy() {
    this.basicEconomySeatsNumber++;
    this.minusBasicEconomyDisabled = false;
  }

  onMinusBasicEconomy() {
    if (this.basicEconomySeatsNumber > 1) {
      this.basicEconomySeatsNumber--;
    }
    if (this.basicEconomySeatsNumber === 1) {
      this.minusBasicEconomyDisabled = true;
    }
  }

  addSeats() {
    if (this.validateSeats()) {
      this.addFirstClass();
      this.addBusiness();
      this.addEconomy();
      this.addBasicEconomy();
      this.addFlight();
    }
  }

  validateSeats() {
    let retVal = true;
    console.log(this.firstClassSeatPrice);
    if (this.firstClassSeatsNumber === null || this.firstClassSeatsNumber < 1) {
      retVal = false;
    }
    if (this.businessSeatsNumber === null || this.businessSeatsNumber < 1) {
      retVal = false;
    }
    if (this.economySeatsNumber === null || this.economySeatsNumber < 1) {
      retVal = false;
    }
    if (this.basicEconomySeatsNumber === null || this.basicEconomySeatsNumber < 1) {
      retVal = false;
    }
    if (this.firstClassSeatPrice === null || this.firstClassSeatPrice < 1) {
      retVal = false;
    }
    if (this.businessSeatPrice === null || this.businessSeatPrice < 1) {
      retVal = false;
    }
    if (this.economySeatPrice === null || this.economySeatPrice < 1) {
      retVal = false;
    }
    if (this.basicEconomySeatPrice === null || this.basicEconomySeatPrice < 1) {
      retVal = false;
    }

    return retVal;
  }

  addFirstClass() {
    const rows = (this.firstClassSeatsNumber % 6 === 0) ? (this.firstClassSeatsNumber / 6) : (this.firstClassSeatsNumber / 6) + 1;
    for (let r = 0; r < rows; r++) {
      for (let c = 0; c < 6; c++) {
        const column = (c === 0) ? 'A' : (c === 1) ? 'B' : (c === 2) ? 'C' : (c === 3) ? 'D' : (c === 4) ? 'E' : 'F';
        this.flight.seats.push(new Seat('F', column, r + 1, this.firstClassSeatPrice));
        this.firstClassSeatsNumber--;
        if (this.firstClassSeatsNumber === 0) {
          return;
        }
      }
    }

  }

  addBusiness() {
    const rows = (this.businessSeatsNumber % 6 === 0) ? (this.businessSeatsNumber / 6) : (this.businessSeatsNumber / 6) + 1;
    for (let r = 0; r < rows; r++) {
      for (let c = 0; c < 6; c++) {
        const column = (c === 0) ? 'A' : (c === 1) ? 'B' : (c === 2) ? 'C' : (c === 3) ? 'D' : (c === 4) ? 'E' : 'F';
        this.flight.seats.push(new Seat('B', column, r + 1, this.businessSeatPrice));
        this.businessSeatsNumber--;
        if (this.businessSeatsNumber === 0) {
          return;
        }
      }
    }

  }

  addEconomy() {
    const rows = (this.economySeatsNumber % 6 === 0) ? (this.economySeatsNumber / 6) : (this.economySeatsNumber / 6) + 1;
    for (let r = 0; r < rows; r++) {
      for (let c = 0; c < 6; c++) {
        const column = (c === 0) ? 'A' : (c === 1) ? 'B' : (c === 2) ? 'C' : (c === 3) ? 'D' : (c === 4) ? 'E' : 'F';
        this.flight.seats.push(new Seat('E', column, r + 1, this.economySeatPrice));
        this.economySeatsNumber--;
        if (this.economySeatsNumber === 0) {
          return;
        }
      }
    }

  }

  addBasicEconomy() {
    const rows = (this.basicEconomySeatsNumber % 6 === 0) ? (this.basicEconomySeatsNumber / 6) : (this.basicEconomySeatsNumber / 6) + 1;
    for (let r = 0; r < rows; r++) {
      for (let c = 0; c < 6; c++) {
        const column = (c === 0) ? 'A' : (c === 1) ? 'B' : (c === 2) ? 'C' : (c === 3) ? 'D' : (c === 4) ? 'E' : 'F';
        this.flight.seats.push(new Seat('BE', column, r + 1, this.basicEconomySeatPrice));
        this.basicEconomySeatsNumber--;
        if (this.basicEconomySeatsNumber === 0) {
          return;
        }
      }
    }

  }

  // OTHER

  nextStep() {
    this.step++;
    this.stepSetup();
    console.log(this.flight);
  }

  previousStep() {
    this.step--;
    this.stepSetup();
  }

  exit() {
    this.router.navigate(['/admin/' + this.adminId + '/flights']);
  }

  stepSetup() {
    if (this.step === 0) {
      // show modal
    }
    if (this.step === 2 && this.flightType === 'with stops') {
      this.loadPickedStops();
    }
    if (this.step === 3) {
      this.flight.stops = this.stops;
    }
    if (this.step === 4) {
      // setTimeout( () => {
      //   this.router.navigate(['/']);
      // }, 300);
    }
  }

  removeErrorClass() {
    this.errorFromLocation = false;
  }

}
