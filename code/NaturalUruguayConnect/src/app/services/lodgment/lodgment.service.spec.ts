import { TestBed } from '@angular/core/testing';

import { LodgmentService } from './lodgment.service';

describe('LodgmentService', () => {
  let service: LodgmentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LodgmentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
