import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {LoginModel} from '../../models/loginModel';
import {Session} from '../../models/session';
import {RouterService} from '../router/router.service';
import {SideNavOption} from '../../models/sideNavOption';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = '';
  private tokenKey = '';
  private readonly tokenSource = new BehaviorSubject<string>(this.tokenKey);
  readonly token$ = this.tokenSource.asObservable();
  private errorMessage = '';
  private readonly errorMessageSource = new BehaviorSubject<string>(this.errorMessage);
  readonly errorMessage$ = this.errorMessageSource.asObservable();
  private userMenuOptions: SideNavOption[] = [];
  private adminUserMenuOptions: SideNavOption[] = [
    {
      icon: 'supervisor_account',
      name: 'Usuarios',
      link: 'users'
    },
    {
      icon: 'library_books',
      name: 'Reservas',
      link: 'bookings'
    },
    {
      icon: 'domain',
      name: 'Puntos turísticos',
      link: 'spots'
    },
    {
      icon: 'hotel',
      name: 'Hospedajes',
      link: 'lodgments'
    },
    {
      icon: 'table_review',
      name: 'Reportes',
      link: 'reports'
    }
  ];
  private guestUserMenuOptions: SideNavOption[] = [
    {
      icon: 'library_books',
      name: 'Consulta de reserva',
      link: '/booking-status'
    }
  ];
  private readonly userMenuOptionsSource = new BehaviorSubject<SideNavOption[]>(this.userMenuOptions);
  readonly userMenuOptions$ = this.userMenuOptionsSource.asObservable();

  constructor(private http: HttpClient, private routerService: RouterService) {
    this.baseUrl = environment.apiUrl;
    this.updateTokenSource();
    this.tokenSource.subscribe(token => {
      if (token !== '') {
        this.userMenuOptionsSource.next(this.adminUserMenuOptions);
      } else {
        this.userMenuOptionsSource.next(this.guestUserMenuOptions);
      }
    });
  }

  private updateTokenSource(): void {
    this.tokenSource.next(localStorage.getItem(this.tokenKey));
  }

  checkAuthenticated(): boolean {
    const authenticated = this.tokenSource.value !== '';
    return authenticated;
  }

  getToken(): string {
    return localStorage.getItem(this.tokenKey);
  }

  login(loginModel: LoginModel): Observable<Session>  {
    const endpoint = this.baseUrl + 'sessions';
    this.errorMessageSource.next('');
    const subscription = this.http.post<Session>(endpoint, loginModel);
    subscription.subscribe(
      (session) => {
      localStorage.setItem(this.tokenKey, session.token);
      this.updateTokenSource();
      },
      (error) => {
        // TODO: Change to some dictionary
        const errorMessage = error.status === 401 ? 'Credenciales inválidas' : 'Error en password';
        this.errorMessageSource.next(errorMessage);
      });
    return subscription;
  }

  logout(redirect: string): void {
    const endpoint = this.baseUrl + 'sessions';
    const headers = { 'Authorization': 'Bearer ' + this.tokenSource.value };
    this.http.delete(endpoint, { headers });
    localStorage.setItem(this.tokenKey, '');
    this.updateTokenSource();
    this.routerService.navigateTo(redirect);
  }
}
