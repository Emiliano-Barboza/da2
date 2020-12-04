import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Page.LandingComponent } from './page.landing.component';

describe('Page.LandingComponent', () => {
  let component: Page.LandingComponent;
  let fixture: ComponentFixture<Page.LandingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Page.LandingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Page.LandingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
