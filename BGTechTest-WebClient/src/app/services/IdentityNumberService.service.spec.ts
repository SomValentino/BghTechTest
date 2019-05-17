/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { IdentityNumberServiceService } from './IdentityNumberService.service';

describe('Service: IdentityNumberService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [IdentityNumberServiceService]
    });
  });

  it('should ...', inject([IdentityNumberServiceService], (service: IdentityNumberServiceService) => {
    expect(service).toBeTruthy();
  }));
});
