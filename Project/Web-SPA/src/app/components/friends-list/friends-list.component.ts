import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/entities/user';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/services/user.service';
import { RegisteredUser } from 'src/app/entities/registeredUser';

@Component({
  selector: 'app-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.scss']
})
export class FriendsListComponent implements OnInit {

  userFriends: Array<RegisteredUser>;
  userId: number;
  user: RegisteredUser;
  temporary: Array<RegisteredUser>;
  usersForSearch: Array<RegisteredUser>;
  searchOn = false;
  addUserOn = false;

  constructor(private route: ActivatedRoute, private userService: UserService) {
    route.params.subscribe(params => { this.userId = params.id; });
  }

  ngOnInit(): void {
    this.user = this.userService.getUser(this.userId);
    this.userFriends = this.user.friendsList;
    this.usersForSearch = this.userFriends;
    this.temporary = this.userFriends;
  }

  showSearchedFriends(searched: Array<RegisteredUser>) {
    this.userFriends = searched;
    this.searchOn = true;
  }

  addFriend() {
    const allUsers = new Array<RegisteredUser>();
    this.userService.getAllUsers().forEach(user => {
      if (!this.userFriends.includes(user) && this.user !== user) {
        allUsers.push(user);
      }
    });

    this.userFriends = allUsers;
    this.usersForSearch = this.userFriends; //sad ide u pretragu svi user-i sistema
    this.addUserOn = true;
  }
  addFriendBack() {
    this.userFriends = this.temporary;
    this.addUserOn = false;
    this.usersForSearch = this.userFriends;
  }
}
