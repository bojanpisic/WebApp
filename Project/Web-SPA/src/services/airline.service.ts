import { Injectable } from '@angular/core';
import { Airline } from 'src/app/entities/airline';
import { Destination } from 'src/app/entities/destination';
import { Flight } from 'src/app/entities/flight';
import { TripId } from 'src/app/entities/trip-id';
import { Address } from 'src/app/entities/address';
import { FlightService } from './flight.service';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AirlineService {

  readonly BaseURI = 'http://localhost:5001/api';

  airlines: Array<Airline>;
  constructor(private flightService: FlightService, private http: HttpClient) {
    this.airlines = new Array<Airline>();
    this.allMockedAirlines();
   }

   test(data: any): Observable<any> {
    console.log(data);
    const param = {
      type: data.type,
      from: data.from,
      to: data.to,
      dep: data.dep,
      ret: data.ret,
      minPrice: data.minPrice,
      maxPrice: data.maxPrice,
      class: data.class,
      air: data.air,
      mind: data.mind,
      maxd: data.maxd
    };
    const url = this.BaseURI + '/airlineadmin/flights';
    return this.http.get<any>(url, {params: param});
  }

  getAirline(data: any): Observable<any> {
    const url = this.BaseURI + '/airlineadmin/get-airline';
    console.log(url);
    return this.http.get<any>(url);
  }

  getAirlines(): Observable<any> {
    const url = this.BaseURI + '/airlineadmin/all-airlines';
    return this.http.get<any>(url);
  }

  getAirlineProfile(data: any) {
    const url = `${this.BaseURI + '/airlineadmin/airline'}/${data}`;
    return this.http.get(url);
  }

  getTopRatedAirlines(): Observable<any> {
    const url = this.BaseURI + '/airlineadmin/get-toprated-airlines';
    console.log(url);
    return this.http.get<any>(url);
  }

  editAirline(data: any) {
    const body = {
      Name: data.name,
      Address: data.address,
      PromoDescription: data.promoDescription,
    };
    const url = this.BaseURI + '/airlineadmin/change-airline-info';
    return this.http.put(url, body);
  }


  getAirlinePhoto(data: any): Observable<any> {
    const url = `${this.BaseURI + '/airlineadmin/get-airline-logo'}/${data}`;
    console.log(url);
    return this.http.get(url);
  }s

  changePhoto(data: any) {
    const formData = new FormData();
    formData.append('img', data.image);

    const url = this.BaseURI + '/airlineadmin/change-airline-logo';
    return this.http.put(url, formData);
  }


  getAdminsFlights(): Observable<any> {
    const url = this.BaseURI + '/airlineadmin/get-flights';
    console.log(url);
    return this.http.get<any>(url);
  }

  getFlightsSeats(data: any): Observable<any> {
    const url = `${this.BaseURI + '/airlineadmin/get-seats'}/${data}`;
    return this.http.get<any>(url);
  }

  addFlight(data: any) {
    console.log(data);
    const body = {
      FlightNumber: data.FlightNumber,
      TakeOffDateTime: data.TakeOffDateTime,
      LandingDateTime: data.LandingDateTime,
      StopIds: data.StopIds, // lista ideja destinacija
      FromId: data.FromId,
      ToId: data.ToId,
      Seats: data.Seats, // Column, Row, Class, Price
      TripLength: data.TripLength
    };
    const url = this.BaseURI + '/airlineadmin/add-flight';
    return this.http.post(url, body);
  }


  getAdminsDestinations(): Observable<any> {
    const url = this.BaseURI + '/airlineadmin/get-airline-destinations';
    console.log(url);
    return this.http.get<any>(url);
  }

  addDestination(data: any) {
    console.log(data);
    const body = {
      State: data.state,
      City: data.city,
      ImgUrl: data.imgUrl,
    };
    const url = this.BaseURI + '/airlineadmin/add-destination';
    return this.http.post(url, body);
  }

  deleteDestination(data: any) {
    const url = this.BaseURI + '/airlineadmin/delete-destination/' + data.id;
    return this.http.delete(url);
  }

  addSeat(data: any) {
    const body = {
      Class: data.class,
      Column: data.column,
      Row: data.row,
      Price: data.price,
      FlightId: data.flightId
    };
    const url = this.BaseURI + '/airlineadmin/add-seat';
    return this.http.post(url, body);
  }

  deleteSeat(data: any) {
    const url = this.BaseURI + '/airlineadmin/delete-seat/' + data;
    return this.http.delete(url);
  }

  changeSeat(data: any) {
    const url = `${this.BaseURI + '/airlineadmin/change-seat'}/${data.id}`;
    const body = {
      Price: data.price,
    };
    return this.http.put(url, body);
  }

  getAdminsSpecialOffers(): Observable<any> {
    const url = this.BaseURI + '/airlineadmin/get-specialoffer';
    console.log(url);
    return this.http.get<any>(url);
  }

  getAirlineSpecialOffers(data): Observable<any> {
    const url = `${this.BaseURI + '/airlineadmin/airline-special-offers'}/${data}`;
    console.log(url);
    return this.http.get<any>(url);
  }

  getAllSpecialOffers(): Observable<any> {
    const url = this.BaseURI + '/airlineadmin/all-airlines-specialoffers';
    console.log(url);
    return this.http.get<any>(url);
  }

  addSpecialOffer(data: any) {
    console.log(data);
    const body = {
      NewPrice: data.NewPrice,
      SeatsIds: data.SeatsIds
    };
    const url = this.BaseURI + '/airlineadmin/add-specialoffer';
    return this.http.post(url, body);
  }

  loadAllAirlines() {
    const retValue = new Array<Airline>();
    for (const item of this.airlines) {
      retValue.push(item);
    }
    return retValue;
  }

  getAdminsAirlineId(adminId: number) {
    let retVal;
    this.airlines.forEach(airline => {
      if (airline.adminid == adminId) {
        retVal = airline.id;
      }
    });
    return retVal;
  }

  getAdminsAirline(adminId: number) {
    let retVal;
    this.airlines.forEach(airline => {
      if (airline.adminid == adminId) {
        retVal = airline;
      }
    });
    return retVal;
  }

  getFlight(airlineId: number, flightNumber: string) {
    let f: Flight;
    this.airlines.forEach(airline => {
      if (airline.id === airlineId) {
        airline.flights.forEach(flight => {
          if (flight.flightNumber === flightNumber) {
            f = flight;
          }
        });
      }
    });
    return f;
  }

  getFlights(airlineId: number) {
    let f;
    this.airlines.forEach(airline => {
      if (airline.id === airlineId) {
        f = airline.flights;
      }
    });
    return f;
  }

  getAirlineId(adminId: number) {
    let retVal;
    this.airlines.forEach(airline => {
      if (airline.adminid === adminId) {
        retVal = airline.id;
      }
    });
    return retVal;
  }

  allMockedAirlines() {
  }
}
