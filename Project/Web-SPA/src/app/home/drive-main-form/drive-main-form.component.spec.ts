import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DriveMainFormComponent } from './drive-main-form.component';

describe('DriveMainFormComponent', () => {
  let component: DriveMainFormComponent;
  let fixture: ComponentFixture<DriveMainFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DriveMainFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DriveMainFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
