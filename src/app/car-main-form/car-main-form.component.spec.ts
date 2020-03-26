import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CarMainFormComponent } from './car-main-form.component';

describe('CarMainFormComponent', () => {
  let component: CarMainFormComponent;
  let fixture: ComponentFixture<CarMainFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CarMainFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CarMainFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
