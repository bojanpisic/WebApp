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

  modal = 'unfriend';
  activeButton = 'all';
  myProps = {friend: undefined, show: false};

  friends: Array<any>;
  nonFriends: Array<any>;
  user: RegisteredUser;
  userId: number;
  searchText = '';

  isOk = false;

  constructor(private route: ActivatedRoute, private userService: UserService) {
    route.params.subscribe(params => {
      this.userId = params.id;
    });
    this.friends = new Array<RegisteredUser>();
    this.nonFriends = new Array<RegisteredUser>();
    this.myProps.show = false;
   }

  ngOnInit(): void {
    this.loadAll();
  }

  loadAll() {
    this.nonFriends = [];
    this.friends = [];
    this.isOk = false;
    const a = this.userService.getNonFriends().subscribe(
      (res: any[]) => {
        if (res.length) {
          res.forEach(element => {
            const b = {
              firstName: element.firstname,
              lastName: element.lastname,
              email: element.email,
              id: element.id
            };
            this.nonFriends.push(b);
          });
          const c = this.userService.getFriends().subscribe(
            (res1: any[]) => {
              if (res1.length) {
                res1.forEach(element1 => {
                  const b1 = {
                    firstName: element1.firstname,
                    lastName: element1.lastname,
                    email: element1.email,
                    id: element1.id
                  };
                  this.friends.push(b1);
                });
                this.isOk = true;
              }
            }
          );
          this.isOk = true;
        }
      }
    );
  }

  focusInput() {
    document.getElementById('searchInput').focus();
  }

  toggleButton(value: string) {
    this.activeButton = value;
  }

  addFriend(id: number) {
    const c = this.userService.addFriend(id).subscribe(
      (res1: any) => {
        this.loadAll();
      },
      err => {
        alert(err.error.description);
      }
    );
  }

  removeFriend(remove: boolean) {
    // const c = this.userService.removeFriend(email).subscribe(
    //   (res1: any) => {
    //     this.loadAll();
    //   },
    //   err => {
    //     alert(err.error.description);
    //   }
    // );
  }

  toggleModal(id: number) {
    this.myProps.friend = this.userService.getUser(id);
    this.myProps.show = !this.myProps.show;
  }
}
