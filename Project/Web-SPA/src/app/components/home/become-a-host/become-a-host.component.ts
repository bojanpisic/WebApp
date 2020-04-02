import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-become-a-host',
  templateUrl: './become-a-host.component.html',
  styleUrls: ['./become-a-host.component.scss']
})
export class BecomeAHostComponent implements OnInit {

  @Input() option: string;

  logged = true;
  username = 'bojanpisic';

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  btnClick() {
    // prvo bi trebalo proveriti da li je vec ulogovan kliknuo na dugme becoma partner
    this.logged === true ? this.router.navigate([this.username + '/' + this.option +  '/register-company']) : 
    this.router.navigate(['notlogged/' + this.option + 'signin']);
  }

}
