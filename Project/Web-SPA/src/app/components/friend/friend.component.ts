import { Component, OnInit, Input } from '@angular/core';
import { Interpolation } from '@angular/compiler';

@Component({
  selector: 'app-friend',
  templateUrl: './friend.component.html',
  styleUrls: ['./friend.component.scss']
})
export class FriendComponent implements OnInit {

  @Input() friend;
  constructor() { }

  ngOnInit(): void {
  }

}
