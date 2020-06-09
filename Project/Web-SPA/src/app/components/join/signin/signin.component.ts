import { Component, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { UserService } from 'src/services/user.service';
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss', '../signup/signup.component.scss']
})

export class SigninComponent implements OnInit {

  username: string;
  option: string;

  loggedIn = false;

  constructor(private route: ActivatedRoute, private router: Router, public userService: UserService) {
    route.params.subscribe(params => {
      this.username = params.username; this.option = params.option; }); //dobija se username onoga ko je kliknuo become a host
  }

  ngOnInit(): void {
    if (localStorage.getItem('token') != null) {

      var token = localStorage.getItem('token');
      var decoded = this.getDecodedAccessToken(token);

      if (token == null || decoded.exp >= Date.now()) {
          alert('Not registered');
          return ;
      }
      console.log(decoded);
      // this.router.navigateByUrl(decoded + '/home');
    }
  }

  signInClick() {
    this.userService.logIn().subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token);
        var decoded = this.getDecodedAccessToken(res.token);

        if (res.token == null || decoded.exp >= Date.now()) {
            alert('Not registered');
            return ;
        }

        switch (decoded.Role) {
          case 'RegularUser':
            this.router.navigateByUrl(decoded.UserID + '/home');
            break;
          case 'ArilineAdmin':
            this.router.navigateByUrl('/admin/' + decoded.UserID);
            break;
          case 'RentCarAdmin':
            this.router.navigateByUrl('/rac-admin/' + decoded.UserID);
            break;
          case 'Admin':
            this.router.navigateByUrl(decoded.UserID + '/home');
            break;
        }

      },
      err => {
        // tslint:disable-next-line: triple-equals
        if (err.status == 400) {
          console.log(err);
          // this.toastr.error('Incorrect username or password.', 'Authentication failed.');
        } else {
          console.log(err);
        }
      }
    );
  }


    getDecodedAccessToken(token: string): any {
      try {
          return jwt_decode(token);
      } catch (Error) {
          return null;
      }
    }

    // let loggedUser = this.userService.logIn((document.getElementById('email') as HTMLInputElement).value,
    // (document.getElementById('password') as HTMLInputElement).value);
    // if (loggedUser) {
    //   alert('you are logged');

    //   (loggedUser.userType === 'airlineAdmin' && this.username === undefined) ?
    //   this.router.navigate(['/airlines/' + loggedUser.id + '/flight-add']) :
    //   (loggedUser.userType === 'regular' && this.username === undefined) ?
    //   (this.router.navigate([loggedUser.id + '/home']), localStorage.setItem(loggedUser.id.toString(), JSON.stringify(loggedUser))) :
    //   this.username === undefined ? this.router.navigate(['/']) :
    //   this.router.navigate([this.username + '/' + this.option + '/register-company']);
    // } else {
    //   alert('logging error');
    // }

}
