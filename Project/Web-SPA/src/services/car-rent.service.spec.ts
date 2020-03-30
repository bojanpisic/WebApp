import { TestBed } from '@angular/core/testing';

import { CarRentService } from '../services/car-rent.service';

describe('CarRentService', () => {
  let service: CarRentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CarRentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
