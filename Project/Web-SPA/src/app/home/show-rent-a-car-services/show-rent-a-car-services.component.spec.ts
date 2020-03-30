import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowRentACarServicesComponent } from './show-rent-a-car-services.component';

describe('ShowRentACarServicesComponent', () => {
  let component: ShowRentACarServicesComponent;
  let fixture: ComponentFixture<ShowRentACarServicesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowRentACarServicesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowRentACarServicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
