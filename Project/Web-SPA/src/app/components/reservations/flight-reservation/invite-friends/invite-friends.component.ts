import { Component, OnInit, Input, Output, EventEmitter, HostListener, ElementRef } from '@angular/core';
import { Flight } from 'src/app/entities/flight';
import { Passenger } from 'src/app/entities/passenger';
import { SeatsForFlight } from 'src/app/entities/seats-for-flight';
import { Seat } from 'src/app/entities/seat';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { RegisteredUser } from 'src/app/entities/registeredUser';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-invite-friends',
  templateUrl: './invite-friends.component.html',
  styleUrls: ['./invite-friends.component.scss']
})
export class InviteFriendsComponent implements OnInit {

  @Input() seat: Seat;
  @Input() flight: Flight;
  @Input() person: Passenger;
  @Output() passenger = new EventEmitter<Passenger>();
  userId: number;
  user: RegisteredUser;

  closeIt = 0;

  firstNameInvalid = false;
  lastNameInvalid = false;
  passportInvalid = false;

  searchText = '';

  option = 'fill';
  btnContent = 'Invite a friend';

  form: FormGroup;

  constructor(private eRef: ElementRef, route: ActivatedRoute, private userService: UserService) {
    route.params.subscribe(params => {
      this.userId = params.id;
    });
  }

  ngOnInit(): void {
    const air1 = this.userService.getUser(this.userId);
    // const airline = this.airlineService.getAdminsAirline(this.adminId);
    // this.companyFields = {
    //   name: airline.name,
    //   location: airline.address,
    //   about: airline.about
    // };
    console.log(air1);
    if (this.person !== undefined) {
      if (this.person.passport === '') {
        this.onClick();
      }
    }
    this.initForm();
    console.log(this.person);
  }

  initForm() {
    this.form = new FormGroup({
      firstName: new FormControl((this.person === undefined) ? '' : this.person.firstName, Validators.required),
      lastName: new FormControl((this.person === undefined) ? '' : this.person.lastName, Validators.required),
      passport: new FormControl((this.person === undefined) ? '' : this.person.passport, Validators.required),
   });
  }

  confirm() {
    // proveriti da li ima putnik sa takvim pasosem na ovom letu
    if (this.validate()) {
      // tslint:disable-next-line:max-line-length
      this.passenger.emit(new Passenger(this.form.controls.firstName.value, this.form.controls.lastName.value, this.form.controls.passport.value));
    }
  }

  validate() {
    let retVal = true;
    if (this.form.controls.firstName.value === '') {
      this.firstNameInvalid = true;
      retVal = false;
    }
    if (this.form.controls.lastName.value === '') {
      this.lastNameInvalid = true;
      retVal = false;
    }
    if (this.form.controls.passport.value === '') {
      this.passportInvalid = true;
      retVal = false;
    }
    return retVal;
  }

  focusInput() {
    document.getElementById('searchInput').focus();
  }

  inviteFriend(friend: RegisteredUser) {
    // tslint:disable-next-line:max-line-length
    this.passenger.emit(new Passenger(friend.firstName, friend.lastName, ''));
  }

  close() {
    if (this.person !== undefined) {
      this.passenger.emit(this.person);
    } else {
      this.passenger.emit(null);
    }
  }

  onClick() {
    this.option = (this.option === 'fill') ? 'invite' : 'fill';
    this.btnContent = (this.btnContent === 'Fill info') ? 'Invite a friend' : 'Fill info';
  }

  onDiscard() {
    this.passenger.emit(null);
  }

  @HostListener('document:click', ['$event'])
  clickout(event) {
    if (this.closeIt < 1) {
      this.closeIt = 1;
    } else {
      this.closeIt = 2;
    }
    if (!this.eRef.nativeElement.contains(event.target)) {
      if (this.closeIt === 2) {
        this.close();
      }
    }
  }

}
