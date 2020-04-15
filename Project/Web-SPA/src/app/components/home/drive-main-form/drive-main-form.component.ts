import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-drive-main-form',
  templateUrl: './drive-main-form.component.html',
  styleUrls: ['./drive-main-form.component.scss', '../flight-main-form/flight-main-form.component.scss']
})
export class DriveMainFormComponent implements OnInit {

  sameLocationChoosed = true;

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  sameLocation() {
    this.sameLocationChoosed = true;
  }

  differentLocation() {
    this.sameLocationChoosed = false;
  }

  onSubmit() {
    this.router.navigate(['/cars']);
  }
}
