import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RedirectToAirlinesComponent } from './redirect-to-airlines.component';

describe('RedirectToAirlinesComponent', () => {
  let component: RedirectToAirlinesComponent;
  let fixture: ComponentFixture<RedirectToAirlinesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RedirectToAirlinesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RedirectToAirlinesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
