import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-places-picker',
  templateUrl: './places-picker.component.html',
  styleUrls: ['./places-picker.component.scss']
})
export class PlacesPickerComponent implements OnInit {

  constructor() { }

  choosenChangeOvers: Array<string>;

  inputStyle = 'margin-top: 10px; \
                margin-right: 3px; \
                width: 20%; \
                height: 30%; \
                display: inline; \
                background-color: rgb(62, 62, 255); \
                color: white; \
                border-radius: 5px; \
                outline: none; \
                border: none; \
                font-size: 12px; \
                text-align: center; \
                padding: 1px;';

  ngOnInit(): void {
    this.choosenChangeOvers = new Array<string>();
  }

  getCityName($event) {
    this.choosenChangeOvers.push($event);
    this.createDivChild();
  }

  createDivChild() {
    let child = document.createElement('input');
    const elementName = this.choosenChangeOvers[this.choosenChangeOvers.length - 1].split(':')[2];
    child.setAttribute('value', elementName);
    child.setAttribute('id', elementName);
    child.setAttribute('type', 'button');
    child.setAttribute('style', this.inputStyle);
    child.addEventListener('click', () => {
        document.getElementById(child.getAttribute('id')).remove();
        this.choosenChangeOvers.splice(this.choosenChangeOvers.indexOf(child.getAttribute('id')), 1);
    });
    document.getElementById('changeover-list').appendChild(child);
  }

}
