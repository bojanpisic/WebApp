import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AirlineInfoComponent } from './airline-info.component';

describe('AirlineInfoComponent', () => {
  let component: AirlineInfoComponent;
  let fixture: ComponentFixture<AirlineInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AirlineInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AirlineInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
