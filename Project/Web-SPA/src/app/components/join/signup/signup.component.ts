import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from 'src/services/user.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {

  step = 'step1';
  registrationType: any; // ovde ce da bude sta se registruje..da li airline admin itd..
  adminId: any; //
  constructor(private route: ActivatedRoute, private router: Router, public userService: UserService) {
    route.params.subscribe(params => {
      this.registrationType = params.registrationType;
      this.adminId = params.id;
    });
   }

  ngOnInit(): void {
    this.userService.formModel.reset();
  }

  next(nextStep: string) {
    this.step = nextStep;

    // if (this.step === 'finish') {
    //   alert("finisg");
    //   setTimeout( () => {
    //     this.router.navigate(['/']);
    //     }, 300);
    // }
  }

  Register() {
    this.userService.userRegistration().subscribe(
      (res: any) => {
        if (res.status === 201) {
          alert('uspeo');
          this.userService.formModel.reset();
        } else {
          alert('nije');

          res.errors.array.forEach(element => {
            switch (element.code)
            {
              case 'DuplicateUserName':
                // this.toastr.error('Username is already taken','Registration failed.');
                break;

              default:
              // this.toastr.error(element.description,'Registration failed.');
                break;
            }
          });
        }
      }
    );
  }
}
