import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/entities/user';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  userId: number;
  user: User;
  somethingChanged = false;
  passwordchanged = false;
  constructor(private route: ActivatedRoute, private userService: UserService) { 
    route.params.subscribe(params => {
      this.userId = params.id;
    });
  }

  ngOnInit(): void {
    this.user = this.userService.getUser(this.userId);
  }

  submitChanges() {
    (document.querySelector('#snackbar') as HTMLElement).className = "show";
    this.somethingChanged = false;
    setTimeout( () => {
        (document.querySelector('#snackbar') as HTMLElement).className = '';
        this.passwordchanged = false;
       }, 2000);
  }

  changePhoto() {
    
  }
  passwordChanged() {
    this.somethingChanged = true;
    this.passwordchanged = true;
  }

}
