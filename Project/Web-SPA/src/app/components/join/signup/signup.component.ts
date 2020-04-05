import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {

  step = 'step1';

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit(): void {
  }

  next(nextStep: string) {
    this.step = nextStep;

    if (this.step === 'finish') {

      setTimeout( () => {
        this.router.navigate(['/']);
        }, 300);
    }
  }

}