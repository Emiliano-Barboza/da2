import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageUsersComponent } from './users.component';

describe('UsersComponent', () => {
  let component: PageUsersComponent;
  let fixture: ComponentFixture<PageUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageUsersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
