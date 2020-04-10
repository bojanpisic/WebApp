import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-airlines-header',
  templateUrl: './airlines-header.component.html',
  styleUrls: ['./airlines-header.component.scss']
})
export class AirlinesHeaderComponent implements OnInit {

  rotateArrow = true;
  nameup = false;
  namedown = false;
  cityup = false;
  citydown = false;

  constructor() { }

  ngOnInit(): void {
  }

  sortClick() {
    this.rotateArrow = !this.rotateArrow;
  }

  sortBy() {
  }

  namedownClicked() {
    if (this.nameup === true) {
      this.nameup = false;
    }
    this.namedown = !this.namedown;
    this.sortBy();
  }

  nameupClicked() {
    if (this.namedown === true) {
      this.namedown = false;
    }
    this.nameup = !this.nameup;
    this.sortBy();
  }

  citydownClicked() {
    if (this.cityup === true) {
      this.cityup = false;
    }
    this.citydown = !this.citydown;
    this.sortBy();
  }

  cityupClicked() {
    if (this.citydown === true) {
      this.citydown = false;
    }
    this.cityup = !this.cityup;
    this.sortBy();
  }

}
