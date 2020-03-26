import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightMainFormComponent } from './flight-main-form.component';

describe('FlightMainFormComponent', () => {
  let component: FlightMainFormComponent;
  let fixture: ComponentFixture<FlightMainFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlightMainFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightMainFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
