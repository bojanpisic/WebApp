import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonNumSearchComponent } from './person-num-search.component';

describe('PersonNumSearchComponent', () => {
  let component: PersonNumSearchComponent;
  let fixture: ComponentFixture<PersonNumSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PersonNumSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonNumSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
