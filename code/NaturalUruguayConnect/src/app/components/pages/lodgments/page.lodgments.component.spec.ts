import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageLodgmentsComponent } from './lodgments.component';

describe('LodgmentsComponent', () => {
  let component: PageLodgmentsComponent;
  let fixture: ComponentFixture<PageLodgmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageLodgmentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageLodgmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
