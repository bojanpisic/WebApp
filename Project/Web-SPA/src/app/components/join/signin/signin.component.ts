import { Component, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { UserService } from 'src/services/user.service';


@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss', '../signup/signup.component.scss']
})

export class SigninComponent implements OnInit {

  username: string;
  option: string;

  loggedIn = false;

  constructor(private route: ActivatedRoute, private router: Router, private userService: UserService) {
    route.params.subscribe(params => {
      this.username = params.username; this.option = params.option; }); //dobija se username onoga ko je kliknuo become a host
  }

  ngOnInit(): void {
  }

  signInClick() {
    let loggedUser = this.userService.logIn((document.getElementById('email') as HTMLInputElement).value,
    (document.getElementById('password') as HTMLInputElement).value);
    if (loggedUser) {
      alert('you are logged');

      (loggedUser.userType === 'airlineAdmin' && this.username === undefined) ?
      this.router.navigate(['/airlines/' + loggedUser.id + '/flight-add']) :
      this.username === undefined ? this.router.navigate(['/']) :
      this.router.navigate([this.username + '/' + this.option + '/register-company']);

    } else {
      alert('logging error');
    }
  }

}
