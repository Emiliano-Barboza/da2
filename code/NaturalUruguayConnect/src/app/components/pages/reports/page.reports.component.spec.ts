import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageReportsComponent } from './reports.component';

describe('ReportsComponent', () => {
  let component: PageReportsComponent;
  let fixture: ComponentFixture<PageReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageReportsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
