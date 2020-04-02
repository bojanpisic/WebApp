import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-company',
  templateUrl: './register-company.component.html',
  styleUrls: ['./register-company.component.scss']
})
export class RegisterCompanyComponent implements OnInit {

  username: string;
  option: string;

  constructor(private route: ActivatedRoute, private router: Router) {
    route.params.subscribe(params => {
      this.username = params.username; this.option = params.option; });
   }

  ngOnInit(): void {
  }

  optionChanged( changedOption: string) {
    this.option = changedOption;
    document.getElementById(this.option).classList.add('choosen-option-class');
    document.getElementById(this.option === 'fly' ? 'drive' : 'fly').classList.remove('choosen-option-class');
  }

}
