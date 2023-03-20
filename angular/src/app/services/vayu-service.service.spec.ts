import { TestBed } from '@angular/core/testing';

import { VayuServiceService } from './vayu-service.service';

describe('VayuServiceService', () => {
  let service: VayuServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VayuServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
