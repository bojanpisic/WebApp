import { Injectable } from '@angular/core';
import { User } from 'src/app/entities/user';
import { ThrowStmt, ConditionalExpr } from '@angular/compiler';
import { RegisteredUser } from 'src/app/entities/registeredUser';
import { Flight } from 'src/app/entities/flight';
import { Destination } from 'src/app/entities/destination';
import { ChangeOver } from 'src/app/entities/changeOver';
import { Seat } from 'src/app/entities/seat';
import { Address } from 'src/app/entities/address';
import { Message } from '../app/entities/message';
import { TripService } from './trip.service';
import { AirlineAdmin } from 'src/app/entities/airlineAdmin';
import { RacAdmin } from 'src/app/entities/racAdmin';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly BaseURI = 'http://192.168.0.13:5001/api';

  constructor(private tripService: TripService, private fb: FormBuilder, private http: HttpClient) {

   }

   formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    FirstName: ['', Validators.required],
    LastName: ['', Validators.required],
    Phone: ['', [Validators.required, Validators.pattern('^[(][+][0-9]{3}[)][0-9]{2}[/][0-9]{3}[-][0-9]{3,4}')]], //validacija
    City: ['', Validators.required],
    Password: ['', [Validators.required, Validators.minLength(4)]],
    ConfirmPassword: ['', Validators.required], //proveriti da li se poklapaju
    ImageUrl: [''],
  });

   formModelLogin = this.fb.group({
    UserNameOrEmail: ['', Validators.required],
    Password: ['', [Validators.required]],
  });


  getUser(data: any): Observable<any> {
    return this.http.get<any>(this.BaseURI + '/profile/get-profile');
    // return this.http.get<any>(this.BaseURI + '/systemadmin/register-systemadmin', body);
  }

  changePhoto(data: any) {
    const formData = new FormData();
    formData.append('img', data.image);

    const url = `${this.BaseURI + '/profile/change-img'}/${data.id}`;
    return this.http.put(url, formData);
  }

  changeFirstName(data: any) {
    const body = {
      FirstName: data.FirstName
    };
    const url = `${this.BaseURI + '/profile/change-firstname'}/${data.id}`;
    return this.http.put(url, body);
  }

  changeLastName(data: any) {
    const body = {
      LastName: data.LastName
    };
    const url = `${this.BaseURI + '/profile/change-lastname'}/${data.id}`;
    return this.http.put(url, body);
  }

  changeEmail(data: any) {
    const body = {
      Email: data.Email
    };
    const url = `${this.BaseURI + '/profile/change-email'}/${data.id}`;
    return this.http.put(url, body);
  }

  changeUsername(data: any) {
    const body = {
      UserName: data.UserName
    };
    const url = `${this.BaseURI + '/profile/change-username'}/${data.id}`;
    return this.http.put(url, body);
  }

  changePhone(data: any) {
    const body = {
      Phone: data.Phone
    };
    const url = `${this.BaseURI + '/profile/change-phone'}/${data.id}`;
    return this.http.put(url, body);
  }

  changeAddress(data: any) {
    const body = {
      City: data.City
    };
    const url = `${this.BaseURI + '/profile/change-city'}/${data.id}`;
    return this.http.put(url, body);
  }

  changePassword(data: any) {
    console.log(data);
    const body = {
      OldPassword: data.OldPassword,
      Password: data.Password,
      PasswordConfirm: data.PasswordConfirm,
    };
    const url = `${this.BaseURI + '/profile/change-passw'}/${data.id}`;
    return this.http.put(url, body);
  }

  comparePasswords(fb: FormGroup) {
    const confirmPswrdCtrl = fb.get('ConfirmPassword');
    // passwordMismatch
    // confirmPswrdCtrl.errors={passwordMismatch:true}
    if (confirmPswrdCtrl.errors == null || 'passwordMismatch' in confirmPswrdCtrl.errors) {
      if (fb.get('Password').value != confirmPswrdCtrl.value) {
        confirmPswrdCtrl.setErrors({ passwordMismatch: true });
      } else {
        confirmPswrdCtrl.setErrors(null);
      }
    }
  }



  AirlineAdminRegistration() {
    // tslint:disable-next-line: prefer-const
    let body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      FirstName: this.formModel.value.FirstName,
      LastName: this.formModel.value.LastName,
      Phone: this.formModel.value.Phone,
      City: this.formModel.value.City,
      ConfirmPassword: this.formModel.value.ConfirmPassword,
      Password: this.formModel.value.Passwords.Password,
    };
    return this.http.post(this.BaseURI + '/authentication/RegisterAirlineAdmin', body);
  }

  userRegistration() {
    // tslint:disable-next-line: prefer-const
    const body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      FirstName: this.formModel.value.FirstName,
      LastName: this.formModel.value.LastName,
      Phone: this.formModel.value.Phone,
      City: this.formModel.value.City,
      ConfirmPassword: this.formModel.value.ConfirmPassword,
      Password: this.formModel.value.Password,
    };
    console.log(body);
    return this.http.post(this.BaseURI + '/authentication/register-user', body);
  }

  RentCarAdminRegistration() {
    // tslint:disable-next-line: prefer-const
    let body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      FirstName: this.formModel.value.FirstName,
      LastName: this.formModel.value.LastName,
      Phone: this.formModel.value.Phone,
      City: this.formModel.value.City,
      ConfirmPassword: this.formModel.value.ConfirmPassword,
      Password: this.formModel.value.Passwords.Password,
    };
    return this.http.post(this.BaseURI + '/authentication/RegisterRentCarAdmin', body);
  }

  SystemAdminRegistration() {
    // tslint:disable-next-line: prefer-const
    const body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      FirstName: this.formModel.value.FirstName,
      LastName: this.formModel.value.LastName,
      Phone: this.formModel.value.Phone,
      City: this.formModel.value.City,
      ConfirmPassword: this.formModel.value.ConfirmPassword,
      Password: this.formModel.value.Password,
    };
    return this.http.post(this.BaseURI + '/authentication/RegisterSytemAdmin', body);
  }

  updateUser(user: RegisteredUser) {
    // const index = this.allUsers.indexOf(user);
    // this.allUsers[index] = user;
  }

  logIn(data: any) {
    const body = {
      UserNameOrEmail: this.formModelLogin.value.UserNameOrEmail,
      Password: this.formModelLogin.value.Password,
      UserId: data.userId,
      Token: data.token
    };
    console.log(body);
    return this.http.post(this.BaseURI + '/authentication/login', body);
  }

  externalLogin(formData) {
    return this.http.post(this.BaseURI + '/ApplicationUser/SocialLogin', formData);
  }

  getAllUsers() {
    // return this.allUsers;
    return null;
  }

  getFriends(id: number) {
    // return this.allUsers.find(x => x.id == id).friends;
  }

  getNonFriends(userId: number, friends: Array<RegisteredUser>) {
    // const retVal = new Array<RegisteredUser>();
    // this.allUsers.forEach(user => {
    //   if (!friends.includes(user) && user.id != userId) {
    //     retVal.push(user);
    //   }
    // });
    // return retVal;
    return null;

  }

  addFriend(userId: number, friendId: number) {
    // const friend = this.getUser(friendId);
    // const user = this.getUser(userId);
    // user.friends.push(friend);
    // const index = this.allUsers.indexOf(user);
    // this.allUsers[index] = user;
    return null;

  }

  removeFriend(userId: number, friendId: number) {
    // const friend = this.getUser(friendId);
    // const user = this.getUser(userId);
    // const indexOfFriend = user.friends.indexOf(friend);
    // user.friends.splice(indexOfFriend, 1);
    // const index = this.allUsers.indexOf(user);
    // this.allUsers[index] = user;
    return null;

  }

  // getUser(id: number) {
  //   // return this.allUsers.find(x => x.id == id);
  //   return null;

  // }

  getAirlineAdmin(id: number) {
    // return this.airlineAdmins.find(x => x.id == id);
    return null;

  }

  getRACAdmin(id: number) {
    // return this.racAdmins.find(x => x.id == id);
    return null;

  }

  roleMatch(allowedRoles): boolean {
    console.log(allowedRoles);
    let isMatch = false;
    const payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
    const userRole = payLoad.Roles;
    console.log(userRole);
    allowedRoles.forEach(element => {
      if (userRole == element) {
        isMatch = true;
      }
    });
    return isMatch;
  }

  hasChangedPassword(): boolean {
    const payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
    console.log(payLoad);
    const passwordChanged = payLoad.PasswordChanged;
    if (passwordChanged === "False") {
      return false;
    }
    return true;
  }

}
