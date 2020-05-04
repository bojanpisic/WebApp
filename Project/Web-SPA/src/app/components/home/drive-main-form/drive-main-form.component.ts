import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-drive-main-form',
  templateUrl: './drive-main-form.component.html',
  styleUrls: ['./drive-main-form.component.scss', '../flight-main-form/flight-main-form.component.scss']
})
export class DriveMainFormComponent implements OnInit {

  sameLocationChoosed = true;
  @Input() userId;

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
    if (this.userId !== undefined) {
      this.router.navigate(['/' + this.userId + '/cars']);
    } else {
      this.router.navigate(['/cars']);
    }
  }
}
