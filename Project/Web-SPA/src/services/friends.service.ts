import { Injectable } from '@angular/core';
import { User } from 'src/app/entities/user';
import { UrlSerializer } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class FriendsService {

  private users: Array<User>;
  constructor() {
    this.users = new Array<User>();
    this.mockedUsers();
  }

  getUsers() {
    return this.users;
  }

  getUser(usrname: string) {
    let us: User;
    this.users.forEach(user => {
      if (user.email === usrname) {
        us = user;
      }
    });
    return us;
  }

  private mockedUsers() {
    const u1 = new User('Bojan', 'Pisic', '', 'bojanpisic', 'male', '00');
    const u2 = new User('Milos', 'Bakmaz', '', 'milosbakmaz', 'male', '00');
    const u3 = new User('Neko', 'Neko', '', 'neko', 'male', '00');
    const u4 = new User('Neko2', 'Neko2', '', 'neko2', 'male', '00');

    u1.friendsList.push(u2);
    u1.friendsList.push(u3);
    u1.friendsList.push(u4);

    this.users.push(u1);
    this.users.push(u2);
    this.users.push(u3);
    this.users.push(u4);

  }
}
