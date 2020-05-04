import { Component, OnInit, Output, HostListener } from '@angular/core';
import { User } from 'src/app/entities/user';
import { UserService } from 'src/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { RegisteredUser } from 'src/app/entities/registeredUser';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.scss']
})
export class FriendsComponent implements OnInit {

  activeButton = 'all';
  myProps = {friend: undefined, show: false};

  friends: Array<RegisteredUser>;
  nonFriends: Array<RegisteredUser>;
  user: RegisteredUser;
  userId: number;
  searchText = '';

  constructor(private route: ActivatedRoute, private userService: UserService) {
    route.params.subscribe(params => {
      this.userId = params.id;
    });
    this.friends = new Array<RegisteredUser>();
    this.nonFriends = new Array<RegisteredUser>();
    this.myProps.show = false;
   }

  ngOnInit(): void {
    this.user = this.userService.getUser(this.userId);
    this.friends = this.user.friends;
    this.nonFriends = this.userService.getNonFriends(this.userId, this.user.friends);
  }

  focusInput() {
    document.getElementById('searchInput').focus();
  }

  toggleButton(value: string) {
    this.activeButton = value;
  }

  addFriend(id: number) {
    this.userService.addFriend(this.userId, id);
    this.user = this.userService.getUser(this.userId);
    this.nonFriends = this.userService.getNonFriends(this.userId, this.user.friends);
    this.friends = this.user.friends;
  }

  removeFriend(remove: boolean) {
    this.myProps.show = !this.myProps.show;
    if (remove) {
      this.userService.removeFriend(this.userId, this.myProps.friend.id);
      this.user = this.userService.getUser(this.userId);
      this.nonFriends = this.userService.getNonFriends(this.userId, this.user.friends);
      this.friends = this.user.friends;
    }
  }

  toggleModal(id: number) {
    this.myProps.friend = this.userService.getUser(id);
    this.myProps.show = !this.myProps.show;
  }
}
