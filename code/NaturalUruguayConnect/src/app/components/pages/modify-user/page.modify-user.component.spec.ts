import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageModifyUserComponent } from './modify-user.component';

describe('ModifyUserComponent', () => {
  let component: PageModifyUserComponent;
  let fixture: ComponentFixture<PageModifyUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageModifyUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageModifyUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
