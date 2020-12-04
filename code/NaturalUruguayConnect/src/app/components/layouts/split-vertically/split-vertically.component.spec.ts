import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SplitVerticallyComponent } from './split-vertically.component';

describe('SplitVerticallyComponent', () => {
  let component: SplitVerticallyComponent;
  let fixture: ComponentFixture<SplitVerticallyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SplitVerticallyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SplitVerticallyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
