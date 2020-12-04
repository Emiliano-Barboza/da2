import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageSpotSetupComponent } from './spot-setup.component';

describe('SpotSetupComponent', () => {
  let component: PageSpotSetupComponent;
  let fixture: ComponentFixture<PageSpotSetupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageSpotSetupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageSpotSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
