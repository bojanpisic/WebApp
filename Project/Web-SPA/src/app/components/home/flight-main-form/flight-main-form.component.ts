import {ComponentRef, ComponentFactoryResolver, ViewContainerRef, ViewChild, Component, OnInit, Renderer2, Input  } from '@angular/core';
import { FlightFormPartComponent } from './flight-form-part/flight-form-part.component';
import { Router } from '@angular/router';
import { MapTypeControlStyle } from '@agm/core/services/google-maps-types';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-flight-main-form',
  templateUrl: './flight-main-form.component.html',
  styleUrls: ['./flight-main-form.component.scss'],
})
export class FlightMainFormComponent implements OnInit {

  @ViewChild('viewContainerRef', { read: ViewContainerRef }) VCR: ViewContainerRef;


  // let.flightType = form.controls['flight-type'].value;
  // u svaki radio input [ngModel]="let.flightType"
  flightType: string;
  @Input() filterForm;

  componentsReferences = [];

  // tslint:disable-next-line:max-line-length
  constructor(private renderer: Renderer2, private CFR: ComponentFactoryResolver, private router: Router) {
   }

  ngOnInit(): void {
    this.flightType = 'oneWayFlight';
  }

  createComponent() {

    const componentFactory = this.CFR.resolveComponentFactory(FlightFormPartComponent);
    const componentRef: ComponentRef<FlightFormPartComponent> = this.VCR.createComponent(componentFactory);
    // const currentComponent = componentRef.instance;

    this.componentsReferences.push(componentRef);
}

  addFlight() {
    this.createComponent();
  }

  removeFlight() {

    if (this.VCR.length < 1) {
            return;
    }

    const componentRef = this.componentsReferences[this.componentsReferences.length - 1];

    this.VCR.remove(this.VCR.length - 1);
    this.componentsReferences.pop();
  }

  oneWay() {
    this.flightType = 'oneWayFlight';
  }

  roundTrip() {
    this.flightType = 'roundTripFlight';
  }

  multiCity() {
    this.flightType = 'multiCityFlight';
  }

  onSubmit() {
    this.router.navigate(['/trips']);
    console.log('SUBMIT');
  }

}
