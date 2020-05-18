import { Injectable } from '@angular/core';
import { User } from 'src/app/entities/user';
import { ThrowStmt } from '@angular/compiler';
import { RegisteredUser } from 'src/app/entities/registeredUser';
import { Flight } from 'src/app/entities/flight';
import { Destination } from 'src/app/entities/destination';
import { ChangeOver } from 'src/app/entities/changeOver';
import { Seat } from 'src/app/entities/seat';
import { Address } from 'src/app/entities/address';
import { Message } from '../app/entities/message';
import { TripService } from './trip.service';
import { AirlineAdmin } from 'src/app/entities/airlineAdmin';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  allUsers = new Array<RegisteredUser>();
  airlineAdmins = new Array<AirlineAdmin>();

  constructor(private tripService: TripService) {
    this.mockedUsers();
    this.mockedAirlineAdmins();
   }

  userRegistration(fname: string, lname: string, password: string,
                   email: string, city: string, phone: string) {
    const newUser = new RegisteredUser(fname, lname, password, email, city, phone);
    newUser.id = this.allUsers.length;

    this.allUsers.push(newUser);
    alert('radi');
  }

  updateUser(user: RegisteredUser) {
    const index = this.allUsers.indexOf(user);
    this.allUsers[index] = user;
  }

  logIn(email: string, pasw: string) {
    let loggedUser: User;
    loggedUser = this.allUsers.find(x => x.email === email && x.password === pasw);
    return loggedUser;
  }

  getAllUsers() {
    return this.allUsers;
  }

  getFriends(id: number) {
    return this.allUsers.find(x => x.id == id).friends;
  }

  getNonFriends(userId: number, friends: Array<RegisteredUser>) {
    const retVal = new Array<RegisteredUser>();
    this.allUsers.forEach(user => {
      if (!friends.includes(user) && user.id != userId) {
        retVal.push(user);
      }
    });
    return retVal;
  }

  addFriend(userId: number, friendId: number) {
    const friend = this.getUser(friendId);
    const user = this.getUser(userId);
    user.friends.push(friend);
    const index = this.allUsers.indexOf(user);
    this.allUsers[index] = user;
  }

  removeFriend(userId: number, friendId: number) {
    const friend = this.getUser(friendId);
    const user = this.getUser(userId);
    const indexOfFriend = user.friends.indexOf(friend);
    user.friends.splice(indexOfFriend, 1);
    const index = this.allUsers.indexOf(user);
    this.allUsers[index] = user;
  }

  getUser(id: number) {
    return this.allUsers.find(x => x.id == id);
  }

  getAirlineAdmin(id: number) {
    return this.airlineAdmins.find(x => x.id == id);
  }

  mockedAirlineAdmins() {
    const user1 = new AirlineAdmin('Milos', 'Bakmaz', 'bakmaz123', 'bakmaz@gmail.com', 'Kings Road 201', '(+381)63/123-456');
    user1.userType = 'admin';
    user1.id = 22;
    user1.airlineId = 0;

    this.airlineAdmins.push(user1);
  }

  mockedUsers() {
    const user1 = new RegisteredUser('Susan', 'William', 'susanwilliam123', 'susan@gmail.com', 'Kings Road 201', '(+381)63/123-456');
    user1.userType = 'regular';
    user1.id = this.allUsers.length;

    const user2 = new RegisteredUser('Harry', 'Smith', 'harrysmith123', 'harry@gmail.com', 'Bridge Street 103', '(+351)24 856-921');
    const user3 = new RegisteredUser('Milos', 'Bakmaz', '', 'milosbakmaz5@gmail.com', '', '');
    const user4 = new RegisteredUser('Marko', 'Bakmaz', '', 'harry@gmail.com', '', '');
    const user5 = new RegisteredUser('Milovan', 'Bakic', '', 'harry@gmail.com', '', '');
    const user6 = new RegisteredUser('Bozidar', 'Bakmaz', '', 'harry@gmail.com', '', '');
    const user7 = new RegisteredUser('Bojan', 'Boric', '', 'harry@gmail.com', '', '');
    const user8 = new RegisteredUser('Ena', 'Ana', '', 'harry@gmail.com', '', '');
    const user9 = new RegisteredUser('Ana', 'Lala', '', 'harry@gmail.com', '', '');
    const user10 = new RegisteredUser('Tuta', 'Fruta', '', 'harry@gmail.com', '', '');

    const f1 = new Flight( 0, new Date(Date.now()), new Date(Date.now()), '03h 40min', 12,
    [new ChangeOver('11:20', '10:30', new Address('Paris', 'France', 'PAR', 0, 0))], 300.00, '234T',
    new Address('Madrid', 'Spain', 'MAD', 0, 0), new Address('Belgrade', 'Serbia', 'BG', 0, 0), '06:20', '12:13',
    [new Seat('F', 'A', 1, 400)]);

    const trips = this.tripService.getAllTrips();

    const message1 = new Message(user2, user1, trips[0], 'Hey, would you like to join me on this trip?');
    const message2 = new Message(user2, user1, trips[1], 'Check out this amazing offer! I would you to join me on this one.');
    const message3 = new Message(user2, user1, trips[2], 'Wanna travel again?');
    const message4 = new Message(user1, user2, trips[1], 'Hey, lets travel together!');

    user1.messages.push(message1);
    user1.messages.push(message2);
    user1.messages.push(message3);
    user1.messages.push(message4);

    user1.flights.push(f1);
    user1.friends.push(user2);
    this.allUsers.push(user1);


    user2.userType = 'regular';
    user2.id = this.allUsers.length;
    this.allUsers.push(user2);

    user3.id = this.allUsers.length;
    this.allUsers.push(user3);
    user4.id = this.allUsers.length;
    this.allUsers.push(user4);
    user5.id = this.allUsers.length;
    this.allUsers.push(user5);
    user6.id = this.allUsers.length;
    this.allUsers.push(user6);
    // user7.id = this.allUsers.length;
    // this.allUsers.push(user7);
    // user8.id = this.allUsers.length;
    // this.allUsers.push(user8);
    // user9.id = this.allUsers.length;
    // this.allUsers.push(user9);
    // user10.id = this.allUsers.length;
    // this.allUsers.push(user10);

    // this.allUsers.push(user5);
    // this.allUsers.push(user6);
    // this.allUsers.push(user7);
    // this.allUsers.push(user8);
    // this.allUsers.push(user9);
    // this.allUsers.push(user10);


    return this.allUsers;
  }
}
