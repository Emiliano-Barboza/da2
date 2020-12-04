import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import {PaginatedModel} from '../../models/paginatedModel';
import {User} from '../../models/user';
import {HttpClient, HttpParams} from '@angular/common/http';
import {AuthService} from '../auth/auth.service';
import {PaginatedCountModel} from '../../models/paginatedCountModel';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = '';
  private user: User = {};
  private readonly userSource = new BehaviorSubject<User>(this.user);
  readonly user$ = this.userSource.asObservable();
  private users: User[] = [];
  private readonly usersSource = new BehaviorSubject<User[]>(this.users);
  readonly users$ = this.usersSource.asObservable();
  private counts: PaginatedCountModel;
  private readonly totalSource = new BehaviorSubject<number>(0);
  readonly total$ = this.totalSource.asObservable();
  private readonly pageSizeSource = new BehaviorSubject<number>(0);
  readonly pageSize$ = this.pageSizeSource.asObservable();

  constructor(private http: HttpClient, private authService: AuthService) {
    this.baseUrl = environment.apiUrl;
  }

  createUser(user: User): Observable<User>  {
    const endpoint = this.baseUrl + 'users';
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken() };
    const subscription = this.http.post<User>(endpoint, user,  { headers });
    return subscription;
  }

  deleteUser(user: User): Observable<User>  {
    const endpoint = this.baseUrl + 'users/' + user.id;
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken() };
    const subscription = this.http.delete<User>(endpoint,  { headers });
    return subscription;
  }

  getUser(id: number): Observable<User> {
    const endpoint = this.baseUrl + 'users/' + id;
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken() };
    const request = this.http.get<User>(endpoint, { headers });
    request.subscribe(response => {
      this.userSource.next(response);
    });
    return request;
  }

  getUsers(params: HttpParams = new HttpParams()): Observable<{data: User[]}> {
    const endpoint = this.baseUrl + 'users';
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken() };
    const request = this.http.get<PaginatedModel>(endpoint, { headers, params });
    request.subscribe(response => {
      this.counts = response.counts;
      this.usersSource.next(response.data);
      this.totalSource.next(this.counts.total);
      this.pageSizeSource.next(this.counts.paging.limit);
    });
    return request;
  }

  updateUser(user: User): Observable<User>  {
    const endpoint = this.baseUrl + 'users/' + user.id;
    const headers = { 'Authorization': 'Bearer ' + this.authService.getToken() };
    const subscription = this.http.put<User>(endpoint, user,  { headers });
    return subscription;
  }
}
