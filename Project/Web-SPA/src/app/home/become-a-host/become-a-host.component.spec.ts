import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BecomeAHostComponent } from './become-a-host.component';

describe('BecomeAHostComponent', () => {
  let component: BecomeAHostComponent;
  let fixture: ComponentFixture<BecomeAHostComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BecomeAHostComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BecomeAHostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
