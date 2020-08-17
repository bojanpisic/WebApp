import { Injectable } from '@angular/core';
import { RentACarService } from 'src/app/entities/rent-a-car-service';
import { Address } from 'src/app/entities/address';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CarRentService {
  readonly BaseURI = 'http://localhost:5001/api';

  cars: Array<{
    brand: string,
    carId: number,
    city: string,
    model: string,
    name: string,
    pricePerDay: number,
    seatsNumber: number,
    state: string,
    type: string,
    year: number,
    priceForSelectedDates: number
  }>;

  constructor(private http: HttpClient) {
    this.cars = [];
    this.mock();
  }

  editRAC(data: any) {
    const body = {
      Name: data.name,
      Address: data.address,
      PromoDescription: data.promoDescription,
    };
    const url = this.BaseURI + '/rentacarserviceadmin/change-racs-info';
    return this.http.put(url, body);
  }

  changeLogo(data: any) {
    const formData = new FormData();
    formData.append('img', data.image);

    const url = this.BaseURI + '/rentacarserviceadmin/change-racs-logo';
    return this.http.put(url, formData);
  }

  getRAC(): Observable<any> {
    const url = this.BaseURI + '/rentacarserviceadmin/get-racs';
    console.log(url);
    return this.http.get<any>(url);
  }

  getRACCityState(): Observable<any> {
    const url = this.BaseURI + '/rentacarserviceadmin/get-racs-address';
    console.log(url);
    return this.http.get<any>(url);
  }

  getRACs(): Observable<any> {
    const url = this.BaseURI + '/home/rent-car-services';
    return this.http.get<any>(url);
  }

  getRACProfile(data: any) {
    const url = `${this.BaseURI + '/home/rent-car-service'}/${data}`;
    return this.http.get(url);
  }

  // *************************************************************************************************************

  addBranch(data: any) {
    console.log(data);
    const body = {
      State: data.State,
      City: data.City,
    };
    const url = this.BaseURI + '/rentacarserviceadmin/add-branch';
    return this.http.post(url, body);
  }

  deleteBranch(data: any) {
    const url = this.BaseURI + '/rentacarserviceadmin/delete-branch/' + data.id;
    return this.http.delete(url);
  }

  getAdminsBranches(): Observable<any> {
    const url = this.BaseURI + '/rentacarserviceadmin/get-racs-branches';
    console.log(url);
    return this.http.get<any>(url);
  }

  // addCarsPhoto(data: any) {
  //   const formData = new FormData();
  //   formData.append('img', data.image);

  //   const url = this.BaseURI + '/rentacarserviceadmin/add-car-img';
  //   return this.http.put(url, formData);

  // }

  // *************************************************************************************************************

  test(data: any): Observable<any> {
    // console.log(data);
    const param = {
      from: data.from,
      to: data.to,
      dep: data.dep,
      ret: data.ret,
      seatFrom: data.seatFrom,
      seatTo: data.seatTo,
      minPrice: data.minPrice,
      maxPrice: data.maxPrice,
      racs: data.racs,
      type: data.type
    };
    const url = this.BaseURI + '/home/cars';
    return this.http.get<any>(url, {params: param});
  }

  getAllCars() {
    alert("mrs");

    return this.cars;
  }

  addCar(data: any) {
    const body = {
      Brand: data.Brand,
      Model: data.Model,
      Year: data.Year,
      Type: data.Type,
      SeatsNumber: data.SeatsNumber,
      PricePerDay: data.PricePerDay,
    };
    const url = this.BaseURI + '/rentacarserviceadmin/add-car';
    return this.http.post(url, body);
  }

  editCar(data: any) {
    const body = {
      Brand: data.Brand,
      Model: data.Model,
      Year: data.Year,
      Type: data.Type,
      SeatsNumber: data.SeatsNumber,
      PricePerDay: data.PricePerDay,
    };
    const url = `${this.BaseURI + '/rentacarserviceadmin/change-car-info'}/${data.id}`;
    return this.http.put(url, body);
  }

  addCarToBranch(data: any) {
    const body = {
      BranchId: data.BranchId,
      Brand: data.Brand,
      Model: data.Model,
      Year: data.Year,
      Type: data.Type,
      SeatsNumber: data.SeatsNumber,
      PricePerDay: data.PricePerDay,
    };
    const url = this.BaseURI + '/rentacarserviceadmin/add-car-to-branch';
    return this.http.post(url, body);
  }

  deleteCar(data: any) {
    const url = this.BaseURI + '/rentacarserviceadmin/delete-car/' + data.id;
    return this.http.delete(url);
  }

  getCar(data: any) {
    const url = `${this.BaseURI + '/rentacarserviceadmin/get-car'}/${data}`;
    return this.http.get(url);
  }

  getAdminsCars(): Observable<any> {
    const url = this.BaseURI + '/rentacarserviceadmin/get-racs-cars';
    return this.http.get<any>(url);
  }

  getBranchesCars(data: any): Observable<any> {
    const url = `${this.BaseURI + '/rentacarserviceadmin/get-branch-cars'}/${data}`;
    return this.http.get<any>(url);
  }

  // *************************************************************************************************************

  addSpecialOffer(data: any) {
    console.log(data);
    const body = {
      FromDate: data.FromDate,
      ToDate: data.ToDate,
      NewPrice: data.NewPrice,
    };
    const url = `${this.BaseURI + '/rentacarserviceadmin/add-car-specialoffer'}/${data.id}`;
    return this.http.post(url, body);
  }

  getAdminsSpecialOffers(): Observable<any> {
    const url = this.BaseURI + '/rentacarserviceadmin/get-cars-specialoffers';
    return this.http.get<any>(url);
  }

  getRACSpecialOffers(data): Observable<any> {
    const url = `${this.BaseURI + '/home/racs-specialoffers'}/${data}`;
    console.log(url);
    return this.http.get<any>(url);
  }

  getAllSpecialOffers(): Observable<any> {
    // let params = {
    //   param1: param1Value,
    //   param2: param2Value
    // };
    // this.router.navigate('/segment1/segment2', { queryParams: params });
    const url = this.BaseURI + '/rentacarserviceadmin/racs-specialoffers';
    console.log(url);
    return this.http.get<any>(url);
  }

  getTotalPriceForResevation(data: any) {
    console.log(data);
    const param = {
      from: data.from,
      to: data.to,
      dep: data.dep,
      ret: data.ret,
      carId: data.carId
    };
    const url = this.BaseURI + '/user/rent-total-price';
    return this.http.get(url, {params: param});
  }

  reserveCar(data) {
    console.log(data);
    const body = {
      CarRentId: data.carId,
      // UserId: data.userId,
      TakeOverCity: data.from,
      ReturnCity: data.to,
      TakeOverDate: data.dep, // pocetni datum
      ReturnDate: data.ret, // datum vracanja auta
      TotalPrice: data.totalPrice, // uzracunata na osnovu cene po danu
    };
    const url = this.BaseURI + '/user/rent-car';
    return this.http.post(url, body);
  }


  allMockedRentServices() {}

  mock() {
    const c1 = {
      brand: 'Range Rover',
      carId: 0,
      city: 'Berlin',
      model: 'Evoque',
      name: 'Hertz',
      pricePerDay: 50,
      seatsNumber: 4,
      state: 'Germany',
      type: 'Luxury',
      year: 2020
    };
    const c2 = {
      brand: 'Range Rover',
      carId: 1,
      city: 'Berlin',
      model: 'Evoque',
      name: 'Hertz',
      pricePerDay: 50,
      seatsNumber: 4,
      state: 'Germany',
      type: 'Luxury',
      year: 2020
    };
    const c3 = {
      brand: 'Range Rover',
      carId: 2,
      city: 'Berlin',
      model: 'Evoque',
      name: 'Hertz',
      pricePerDay: 50,
      seatsNumber: 4,
      state: 'Germany',
      type: 'Luxury',
      year: 2020
    };
    const c4 = {
      brand: 'Range Rover',
      carId: 3,
      city: 'Berlin',
      model: 'Evoque',
      name: 'Hertz',
      pricePerDay: 50,
      seatsNumber: 4,
      state: 'Germany',
      type: 'Luxury',
      year: 2020
    };

    // this.cars.push(c1);
    // this.cars.push(c2);
    // this.cars.push(c3);
    // this.cars.push(c4);
  }
}
