import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageSpotsComponent } from './spots.component';

describe('SpotsComponent', () => {
  let component: PageSpotsComponent;
  let fixture: ComponentFixture<PageSpotsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageSpotsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageSpotsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
