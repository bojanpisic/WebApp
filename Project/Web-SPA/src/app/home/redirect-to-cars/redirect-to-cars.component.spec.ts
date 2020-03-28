import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RedirectToCarsComponent } from './redirect-to-cars.component';

describe('RedirectToCarsComponent', () => {
  let component: RedirectToCarsComponent;
  let fixture: ComponentFixture<RedirectToCarsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RedirectToCarsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RedirectToCarsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
