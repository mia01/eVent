import { TestBed } from '@angular/core/testing';

import { CustomErrorHandler } from './custom-error.handler';

describe('CustomErrorHandler', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CustomErrorHandler = TestBed.get(CustomErrorHandler);
    expect(service).toBeTruthy();
  });
});
