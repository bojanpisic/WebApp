import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/entities/user';
import { FriendsService } from 'src/services/friends.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.scss']
})
export class FriendsListComponent implements OnInit {

  userFriends: Array<User>;
  userId: string;

  constructor(private route: ActivatedRoute, private friendService: FriendsService) {
    route.params.subscribe(params => { this.userId = params.id; });
  }

  ngOnInit(): void {
    this.userFriends = this.friendService.getUser(this.userId).friendsList;
  }

}
