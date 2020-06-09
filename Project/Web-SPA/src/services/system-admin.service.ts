import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SystemAdminService {

  readonly BaseURI = 'http://192.168.0.13:5001/api';

  constructor(private http: HttpClient) { }

  registerAirline(data: any) {
    const body = {
      UserName: data.userName,
      Email: data.email,
      ConfirmPassword: data.confirmPassword,
      Password: data.password,
      Name: data.companyName,
      Address: {city: 'Paris', state: 'France', lat: 0.05, lon: 3}
    };
    return this.http.post(this.BaseURI + '/systemadmin/register-airline', body);
  }

  registerRACService(data: any): Observable<any> {
    const body = {
      UserName: data.userName,
      Email: data.email,
      FirstName: data.firstName,
      LastName: data.lastName,
      Phone: data.phone,
      City: data.city,
      ConfirmPassword: data.confirmPassword,
      Password: data.password,
      AirlineName: data.airlineName,
      AirlineAddress: data.airlineAddress
    };
    return this.http.post(this.BaseURI + '/authentication/register-rac-service', body)
      .pipe(
        catchError(this.handleError)
      );
  }

  get(): Observable<any> {
    return this.http.get<any>(this.BaseURI + '/test');
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return throwError(
      'Something bad happened; please try again later.');
  }
}
