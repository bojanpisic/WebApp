import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SystemAdminService } from 'src/services/system-admin.service';
import { CustomValidationService } from 'src/services/custom-validation.service';

@Component({
  selector: 'app-company',
  templateUrl: './add-company.component.html',
  styleUrls: ['./add-company.component.scss']
})
export class AddCompanyComponent implements OnInit {

  adminId: number;
  companyType: string;

  formUser: FormGroup;
  formCompany: FormGroup;

  step = 1;

  errorFirstName = false;
  errorLastName = false;
  errorEmail = false;
  errorUsername = false;
  errorPassword = false;
  errorConfirmPassword = false;
  errorCity = false;
  errorPhone = false;
  errorMsg = '';

  constructor(private route: ActivatedRoute, private router: Router,
              public adminService: SystemAdminService, private formBuilder: FormBuilder,
              private customValidator: CustomValidationService) {
    route.params.subscribe(params => {
      this.adminId = params.id;
      this.companyType = params.type;
    });
  }

  ngOnInit(): void {
    this.initFormUser();
    this.initFormCompany();
  }


  onRegister() {
    const data = {
      email: this.formUser.controls.email.value,
      userName: this.formUser.controls.username.value,
      password: this.formUser.controls.password.value,
      confirmPassword: this.formUser.controls.confirmPassword.value,
      companyName: this.formCompany.controls.name.value,
      companyAddress: this.formCompany.controls.address.value
    };
    this.adminService.registerAirline(data).subscribe();
  }

  onSubmitUser() {
    if (this.validateUserForm()) {
      this.step = 2;
    }
  }

  onSubmitCompany() {
    if (this.formCompany.valid) {
      // this.adminService.get().subscribe();
      this.step = 3;
    }
  }

  onExit() {
    this.router.navigate(['/system-admin/' + this.adminId]);
  }

  onBack() {
    this.step--;
  }

  initFormUser() {
    this.formUser = this.formBuilder.group({
      email: ['', [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$')]],
      username: ['', Validators.required],
      password: ['', [Validators.required, this.customValidator.patternValidator()]],
      confirmPassword: ['', Validators.required],
   }, {
     validators: this.passwords.bind(this)
   });
  }

  passwords(formGroup: FormGroup) {
    const { value: password} = formGroup.get('password');
    const { value: confirmPassword} = formGroup.get('confirmPassword');
    return password === confirmPassword ? null : { passwordNotMatch: true };
  }


  initFormCompany() {
    this.formCompany = new FormGroup({
      name: new FormControl('', Validators.required),
      address: new FormControl('', Validators.required),
   });
  }

  validateUserForm() {
    let retVal = true;
    if (this.formUser.controls.email.invalid) {
      this.errorEmail = true;
      retVal = false;
    }
    if (this.formUser.controls.username.invalid) {
      this.errorUsername = true;
      retVal = false;
    }
    if (this.formUser.controls.password.invalid) {
      this.errorPassword = true;
      retVal = false;
    }
    if (this.formUser.controls.confirmPassword.invalid) {
      this.errorConfirmPassword = true;
      retVal = false;
    }
    console.log('retval' + retVal);
    return retVal;
  }

}
