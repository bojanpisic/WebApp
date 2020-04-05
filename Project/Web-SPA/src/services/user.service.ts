import { Injectable } from '@angular/core';
import { User } from 'src/app/entities/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  allUsers = new Array<User>();

  constructor() {
    this.mockedUsers();
   }

  userRegistration(fname: string, lname: string, password: string,
                   email: string, city: string, phone: string) {
    const newUser = new User(fname, lname, password, email, city, phone);
    newUser.id = this.allUsers.length;

    this.allUsers.push(newUser);
    alert('radi');
  }

  logIn(email: string, pasw: string) {
    let loggedUser: User;
    this.allUsers.forEach(element => {
      if (element.email === email && element.password === pasw) {
        loggedUser = element;
      }
    });
    return loggedUser;
  }

  getAllUsers() {
    return this.allUsers;
  }

  mockedUsers() {
    const user1 = new User('Bojan', 'Pisic', '123456', 'bojanpisic', 'Rocevic', '00000000');
    user1.userType = 'airlineAdmin';
    user1.id = this.allUsers.length;
    this.allUsers.push(user1);

    const user2 = new User('Bojan', 'Pisic', '123456', 'email', 'Rocevic', '00000000');
    user2.userType = 'regular';
    user2.id = this.allUsers.length;
    this.allUsers.push(user2);

    return this.allUsers;
  }
}
