import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageAddUserComponent } from './add-user.component';

describe('AddUserComponent', () => {
  let component: PageAddUserComponent;
  let fixture: ComponentFixture<PageAddUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageAddUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageAddUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
