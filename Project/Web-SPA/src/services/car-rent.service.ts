import { Injectable } from '@angular/core';
import { RentACarService } from 'src/app/entities/rent-a-car-service';
import { Address } from 'src/app/entities/address';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CarRentService {
  readonly BaseURI = 'http://192.168.0.13:5001/api';

  constructor(private http: HttpClient) {}

  editRAC(data: any) {
    const body = {
      Name: data.name,
      Address: data.address,
      PromoDescription: data.promoDescription,
    };
    const url = this.BaseURI + '/rentcarservice/change-racs-info';
    return this.http.put(url, body);
  }

  changeLogo(data: any) {
    const formData = new FormData();
    formData.append('img', data.image);

    const url = this.BaseURI + '/rentcarservice/change-racs-logo';
    return this.http.put(url, formData);
  }

  getRAC(): Observable<any> {
    const url = this.BaseURI + '/rentcarservice/get-racs';
    console.log(url);
    return this.http.get<any>(url);
  }

  getRACCityState(): Observable<any> {
    const url = this.BaseURI + '/rentcarservice/get-racs-address';
    console.log(url);
    return this.http.get<any>(url);
  }

  getRACs(): Observable<any> {
    const url = this.BaseURI + '/rentcarservice/rent-car-services';
    return this.http.get<any>(url);
  }

  getRACProfile(data: any) {
    const url = `${this.BaseURI + '/rentcarservice/rent-car-service'}/${data}`;
    return this.http.get(url);
  }

  // *************************************************************************************************************

  addBranch(data: any) {
    console.log(data);
    const body = {
      State: data.State,
      City: data.City,
    };
    const url = this.BaseURI + '/rentcarservice/add-branch';
    return this.http.post(url, body);
  }

  deleteBranch(data: any) {
    const url = this.BaseURI + '/rentcarservice/delete-branch/' + data.id;
    return this.http.delete(url);
  }

  getAdminsBranches(): Observable<any> {
    const url = this.BaseURI + '/rentcarservice/get-racs-branches';
    console.log(url);
    return this.http.get<any>(url);
  }

  // addCarsPhoto(data: any) {
  //   const formData = new FormData();
  //   formData.append('img', data.image);

  //   const url = this.BaseURI + '/rentcarservice/add-car-img';
  //   return this.http.put(url, formData);

  // }

  // *************************************************************************************************************

  test(data: any): Observable<any> {
    console.log(data);
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
    const url = this.BaseURI + '/rentcarservice/cars';
    return this.http.get<any>(url, {params: param});
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
    const url = this.BaseURI + '/rentcarservice/add-car';
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
    const url = `${this.BaseURI + '/rentcarservice/change-car-info'}/${data.id}`;
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
    const url = this.BaseURI + '/rentcarservice/add-car-to-branch';
    return this.http.post(url, body);
  }

  deleteCar(data: any) {
    const url = this.BaseURI + '/rentcarservice/delete-car/' + data.id;
    return this.http.delete(url);
  }

  getCar(data: any) {
    const url = `${this.BaseURI + '/rentcarservice/get-car'}/${data}`;
    return this.http.get(url);
  }

  getAdminsCars(): Observable<any> {
    const url = this.BaseURI + '/rentcarservice/get-racs-cars';
    return this.http.get<any>(url);
  }

  getBranchesCars(data: any): Observable<any> {
    const url = `${this.BaseURI + '/rentcarservice/get-branch-cars'}/${data}`;
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
    const url = `${this.BaseURI + '/rentcarservice/add-car-specialoffer'}/${data.id}`;
    return this.http.post(url, body);
  }

  getAdminsSpecialOffers(): Observable<any> {
    const url = this.BaseURI + '/rentcarservice/get-cars-specialoffers';
    return this.http.get<any>(url);
  }

  getRACSpecialOffers(data): Observable<any> {
    const url = `${this.BaseURI + '/rentcarservice/racs-specialoffers'}/${data}`;
    console.log(url);
    return this.http.get<any>(url);
  }

  getAllSpecialOffers(): Observable<any> {
    // let params = {
    //   param1: param1Value,
    //   param2: param2Value
    // };
    // this.router.navigate('/segment1/segment2', { queryParams: params });
    const url = this.BaseURI + '/rentcarservice/racs-specialoffers';
    console.log(url);
    return this.http.get<any>(url);
  }


  allMockedRentServices() {}
}
