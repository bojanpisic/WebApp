import { Injectable } from '@angular/core';
import { User } from 'src/app/entities/user';
import { ThrowStmt } from '@angular/compiler';
import { RegisteredUser } from 'src/app/entities/registeredUser';
import { Flight } from 'src/app/entities/flight';
import { Destination } from 'src/app/entities/destination';
import { ChangeOver } from 'src/app/entities/changeOver';
import { Seat } from 'src/app/entities/seat';
import { Address } from 'src/app/entities/address';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  allUsers = new Array<RegisteredUser>();

  constructor() {
    this.mockedUsers();
   }

  userRegistration(fname: string, lname: string, password: string,
                   email: string, city: string, phone: string) {
    const newUser = new RegisteredUser(fname, lname, password, email, city, phone);
    newUser.id = this.allUsers.length;

    this.allUsers.push(newUser);
    alert('radi');
  }

  logIn(email: string, pasw: string) {
    let loggedUser: User;
    loggedUser = this.allUsers.find(x => x.email === email && x.password === pasw);
    return loggedUser;
  }

  getAllUsers() {
    return this.allUsers;
  }

  getUser(id: number) {
    return this.allUsers.find(x => x.id == id);
  }

  mockedUsers() {
    const user1 = new RegisteredUser('Susan', 'William', 'susanwilliam123', 'susan@gmail.com', 'Kings Road 201', '(+381)63/123-456');
    user1.userType = 'regular';
    user1.id = this.allUsers.length;

    const user2 = new RegisteredUser('Harry', 'Smith', 'harrysmith', 'harry@gmail.com', 'Bridge Street 103', '(+351)24 856-921');

    const f1 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '03h 40min', 12,
    [new ChangeOver('11:20', '10:30', new Address('Paris', 'France', 'PAR', 0, 0))], 300.00, '234T',
    new Address('Madrid', 'Spain', 'MAD', 0, 0), new Address('Belgrade', 'Serbia', 'BG', 0, 0), '06:20', '12:13',
    [new Seat(0, '33R')]);

    user1.flights.push(f1);
    user1.friendsList.push(user2);
    this.allUsers.push(user1);


    user2.userType = 'regular';
    user2.id = this.allUsers.length;


    this.allUsers.push(user2);

    return this.allUsers;
  }
}
