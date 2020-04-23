import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/entities/user';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/services/user.service';
import { FormGroup, FormControl, Validators, ValidatorFn, AbstractControl} from '@angular/forms';
import { CustomValidationService } from 'src/services/custom-validation.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss'],
})
export class EditProfileComponent implements OnInit {

  confirmPasswordType = 'password';
  confirmPasswordPicture = '../../../../../assets/visibility-black-18dp.svg';
  passwordMatch = false;

  showFirstName = false;
  showLastName = false;
  showEMail = false;
  showPassword = false;
  showPhone = false;
  showAddress = false;

  btnFirstNameEnabled = true;
  btnLastNameEnabled = true;
  btnEMailEnabled = true;
  btnPasswordEnabled = true;
  btnPhoneEnabled = true;
  btnAddressEnabled = true;

  form: FormGroup;

  userId: number;
  user: User;

  constructor(private route: ActivatedRoute, private userService: UserService, private customValidator: CustomValidationService) {
    route.params.subscribe(params => {
      this.userId = params.id;
    });
  }

  ngOnInit(): void {
    this.user = this.userService.getUser(this.userId);
    this.initForm();
  }

  initForm() {
    this.form = new FormGroup({
      firstName: new FormControl(this.user.firstName, Validators.required),
      lastName: new FormControl(this.user.lastName, Validators.required),
      email: new FormControl(this.user.email, [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$')]),
      password: new FormControl(this.user.password, [Validators.required, this.customValidator.patternValidator()]),
      confirmPassword: new FormControl(''),
      phone: new FormControl(this.user.phone, [Validators.required,
                                               Validators.pattern('^[(][+][0-9]{3}[)][0-9]{2}[/][0-9]{3}[-][0-9]{3,4}')]),
      address: new FormControl(this.user.address, Validators.required),
    });
  }

  editFirstName() {
    this.showFirstName = !this.showFirstName;
    this.btnLastNameEnabled = !this.btnLastNameEnabled;
    this.btnEMailEnabled = !this.btnEMailEnabled;
    this.btnPasswordEnabled = !this.btnPasswordEnabled;
    this.btnPhoneEnabled = !this.btnPhoneEnabled;
    this.btnAddressEnabled = !this.btnAddressEnabled;
    this.initForm();
  }

  editLastName() {
    this.showLastName = !this.showLastName;
    this.btnFirstNameEnabled = !this.btnFirstNameEnabled;
    this.btnEMailEnabled = !this.btnEMailEnabled;
    this.btnPasswordEnabled = !this.btnPasswordEnabled;
    this.btnPhoneEnabled = !this.btnPhoneEnabled;
    this.btnAddressEnabled = !this.btnAddressEnabled;
    this.initForm();
  }

  editEMail() {
    this.showEMail = !this.showEMail;
    this.btnLastNameEnabled = !this.btnLastNameEnabled;
    this.btnFirstNameEnabled = !this.btnFirstNameEnabled;
    this.btnPasswordEnabled = !this.btnPasswordEnabled;
    this.btnPhoneEnabled = !this.btnPhoneEnabled;
    this.btnAddressEnabled = !this.btnAddressEnabled;
    this.initForm();
  }

  editPassword() {
    this.passwordMatch = false;
    this.showPassword = !this.showPassword;
    this.btnLastNameEnabled = !this.btnLastNameEnabled;
    this.btnEMailEnabled = !this.btnEMailEnabled;
    this.btnFirstNameEnabled = !this.btnFirstNameEnabled;
    this.btnPhoneEnabled = !this.btnPhoneEnabled;
    this.btnAddressEnabled = !this.btnAddressEnabled;
    this.initForm();
  }

  editPhone() {
    this.showPhone = !this.showPhone;
    this.btnLastNameEnabled = !this.btnLastNameEnabled;
    this.btnEMailEnabled = !this.btnEMailEnabled;
    this.btnPasswordEnabled = !this.btnPasswordEnabled;
    this.btnFirstNameEnabled = !this.btnFirstNameEnabled;
    this.btnAddressEnabled = !this.btnAddressEnabled;
    this.initForm();
  }

  editAddress() {
    this.showAddress = !this.showAddress;
    this.btnLastNameEnabled = !this.btnLastNameEnabled;
    this.btnEMailEnabled = !this.btnEMailEnabled;
    this.btnPasswordEnabled = !this.btnPasswordEnabled;
    this.btnPhoneEnabled = !this.btnPhoneEnabled;
    this.btnFirstNameEnabled = !this.btnFirstNameEnabled;
    this.initForm();
  }

  saveFirstName() {
    if (!this.form.controls.firstName.invalid) {
      this.user.firstName = this.form.controls.firstName.value;
    }
    this.editFirstName();
  }

  saveLastName() {
    if (!this.form.controls.lastName.invalid) {
      this.user.lastName = this.form.controls.lastName.value;
    }
    this.editLastName();
  }

  saveEMail() {
    if (!this.form.controls.email.invalid) {
      this.user.email = this.form.controls.email.value;
    }
    this.editEMail();
  }

  savePassword() {
    if (this.form.controls.password.value === this.form.controls.confirmPassword.value) {
      if (!this.form.controls.password.invalid) {
        this.user.password = this.form.controls.password.value;
      }
      this.editPassword();
      this.passwordMatch = false;
    } else {
      this.passwordMatch = true;
    }
  }

  savePhone() {
    if (!this.form.controls.phone.invalid) {
      this.user.phone = this.form.controls.phone.value;
    }
    this.editPhone();
  }

  saveAddress() {
    if (!this.form.controls.address.invalid) {
      this.user.address = this.form.controls.address.value;
    }
    this.editAddress();
  }

  toggleEyePicture() {
    this.confirmPasswordType = (this.confirmPasswordType === 'password') ? 'text' : 'password';
    this.confirmPasswordPicture = (this.confirmPasswordPicture === '../../../../../assets/visibility-black-18dp.svg')
                                  ? '../../../../../assets/visibility_off-black-18dp.svg'
                                  : '../../../../../assets/visibility-black-18dp.svg';
  }
}
