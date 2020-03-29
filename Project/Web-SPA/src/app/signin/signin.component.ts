import { Component, OnInit} from '@angular/core';
import { element } from 'protractor';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss', '../signup/signup.component.scss']
})

export class SigninComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
}
