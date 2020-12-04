import {Component, OnInit, ViewChild} from '@angular/core';
import {AuthService} from '../../../services/auth/auth.service';
import {Observable} from 'rxjs';
import {SideNavOption} from '../../../models/sideNavOption';

@Component({
  selector: 'app-page-landing',
  templateUrl: './page.landing.component.html',
  styleUrls: ['./page.landing.component.css']
})
export class LandingComponent implements OnInit {
  private rootPath = '';
  loginLabel = 'Login';
  logoutLabel = 'Logout';
  bookingStatusLabel = 'Consulta de reserva';
  opened: boolean;
  sideNavOptions: Observable<SideNavOption[]>;
  isLogged = false;

  @ViewChild('sidenav') sidebar;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.sideNavOptions = this.authService.userMenuOptions$;
    this.authService.token$.subscribe(token => {
      this.isLogged = token !== '';
    });
  }

  logout(): void {
    this.toggleSidebar();
    this.authService.logout(this.rootPath);
  }

  toggleSidebar(): void {
    this.sidebar.toggle();
  }
}
