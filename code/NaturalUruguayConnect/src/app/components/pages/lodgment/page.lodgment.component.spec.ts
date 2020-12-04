import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageLodgmentComponent } from './lodgment.component';

describe('LodgmentComponent', () => {
  let component: PageLodgmentComponent;
  let fixture: ComponentFixture<PageLodgmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageLodgmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageLodgmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
