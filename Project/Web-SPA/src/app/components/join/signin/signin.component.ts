import { Component, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss', '../signup/signup.component.scss']
})

export class SigninComponent implements OnInit {

  username: string;
  option: string;
  constructor(private route: ActivatedRoute, private router: Router) {
    route.params.subscribe(params => {
      this.username = params.username; this.option = params.option; }); //dobija se username onoga ko je kliknuo become a host
  }

  ngOnInit(): void {
  }

  signInClick() {
    this.username === undefined ? this.router.navigate(['/']) :
                      this.router.navigate([this.username + '/' + this.option + '/register-company']);

  }
}
