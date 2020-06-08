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

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly BaseURI = 'http://localhost:5001/api';

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
    return this.http.post(this.BaseURI + '/authentication/RegisterUser', body);
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

  logIn() {
    const body = {
      UserNameOrEmail: this.formModelLogin.value.UserNameOrEmail,
      Password: this.formModelLogin.value.Password,
    };
    console.log(body);
    return this.http.post(this.BaseURI + '/authentication/Login', body);
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

  getUser(id: number) {
    // return this.allUsers.find(x => x.id == id);
    return null;

  }

  getAirlineAdmin(id: number) {
    // return this.airlineAdmins.find(x => x.id == id);
    return null;

  }

  getRACAdmin(id: number) {
    // return this.racAdmins.find(x => x.id == id);
    return null;

  }

}
