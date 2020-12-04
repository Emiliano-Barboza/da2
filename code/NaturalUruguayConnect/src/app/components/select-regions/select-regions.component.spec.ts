import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectRegionsComponent } from './select-regions.component';

describe('SelectRegionsComponent', () => {
  let component: SelectRegionsComponent;
  let fixture: ComponentFixture<SelectRegionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectRegionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectRegionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
