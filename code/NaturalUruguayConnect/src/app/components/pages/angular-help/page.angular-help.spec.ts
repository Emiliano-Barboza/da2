import { TestBed } from '@angular/core/testing';
import { PageAngularHelpComponent } from './page.angular-help';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        PageAngularHelpComponent
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(PageAngularHelpComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'NaturalUruguayConnect'`, () => {
    const fixture = TestBed.createComponent(PageAngularHelpComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('NaturalUruguayConnect');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(PageAngularHelpComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.content span').textContent).toContain('NaturalUruguayConnect app is running!');
  });
});
