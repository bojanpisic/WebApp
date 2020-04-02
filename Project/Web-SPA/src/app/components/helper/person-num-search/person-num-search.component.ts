import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-person-num-search',
  templateUrl: './person-num-search.component.html',
  styleUrls: ['./person-num-search.component.scss']
})
export class PersonNumSearchComponent implements OnInit {

  rotateArrow = false;
  numOfChildren = 0;
  numOfAdults = 1;
  buttonContent = '1 adult';

  constructor() { }

  ngOnInit(): void {
  }

  travellersBtnClick() {
    this.rotateArrow = this.rotateArrow === true ? false : true;

    if (this.rotateArrow) {
      document.getElementById('travellers-box').classList.remove('hide-travellers-box');
      document.getElementById('travellers-box').classList.add('show-travellers-box');
    } else {
      document.getElementById('travellers-box').classList.add('hide-travellers-box');
      document.getElementById('travellers-box').classList.remove('show-travellers-box');
    }
  }

  minus(type: string) {
    if (type === 'adults' && this.numOfAdults > 1) {
      this.numOfAdults = this.numOfAdults - 1;
    } else if (type === 'children' && this.numOfChildren  >= 1) {
      this.numOfChildren = this.numOfChildren - 1;
    }

    this.changeContentOfBtn();
  }

  plus(type: string) {
    if (type === 'adults') {
      this.numOfAdults = this.numOfAdults + 1;
    } else {
      this.numOfChildren = this.numOfChildren + 1;
    }

    this.changeContentOfBtn();
  }

  changeContentOfBtn() {
    if (this.numOfChildren === 0) {
      this.buttonContent = this.numOfAdults.toString() + ' adults';
    } else {
      this.buttonContent = this.numOfAdults.toString() + ' adults, ' + this.numOfChildren.toString() + ' children';
    }
  }

}
