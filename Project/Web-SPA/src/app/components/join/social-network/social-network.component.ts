import { Component, OnInit } from '@angular/core';
import { AuthService, GoogleLoginProvider, FacebookLoginProvider } from 'angular-6-social-login';
import { UserService } from 'src/services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-social-network',
  templateUrl: './social-network.component.html',
  styleUrls: ['./social-network.component.scss']
})
export class SocialNetworkComponent implements OnInit {

  constructor(public OAuth: AuthService, public userService: UserService, private router: Router) { }

  ngOnInit(): void {
  }

  LoginWithGoogle() {
    let socialPlatformProvider;
    socialPlatformProvider = GoogleLoginProvider.PROVIDER_ID;

    this.OAuth.signIn(socialPlatformProvider).then(socialusers => {
      console.log(socialusers);
      this.userService.externalLogin(socialusers).subscribe(
        (res: any) => {
        localStorage.setItem('token', res.token);
        this.router.navigateByUrl('/home');
      });

      console.log(socialusers);
    });

  }

  LoginWithFacebook() {
    let socialPlatformProvider;
    socialPlatformProvider = FacebookLoginProvider.PROVIDER_ID;

    this.OAuth.signIn(socialPlatformProvider).then(socialusers => {
      console.log(socialusers);

      this.userService.externalLogin(socialusers).subscribe(
        (res: any) => {
        localStorage.setItem('token', res.token);
        this.router.navigateByUrl('/home');
      });

      console.log(socialusers);
    });

  }
}
