import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageLodgmentSetupComponent } from './lodgment-setup.component';

describe('LodgmentSetupComponent', () => {
  let component: PageLodgmentSetupComponent;
  let fixture: ComponentFixture<PageLodgmentSetupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageLodgmentSetupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageLodgmentSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
