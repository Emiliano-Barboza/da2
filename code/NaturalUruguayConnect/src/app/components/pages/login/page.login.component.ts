import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AuthService} from '../../../services/auth/auth.service';
import {LoginModel} from '../../../models/loginModel';
import {Observable} from 'rxjs';
import {RouterService} from '../../../services/router/router.service';

@Component({
  selector: 'app-login',
  templateUrl: './page.login.component.html',
  styleUrls: ['./page.login.component.css']
})
export class PageLoginComponent implements OnInit {
  private returnUrl: string;
  private rootPath = '/';
  form: FormGroup;
  titleLabel = 'Uruguay Natural';
  loginLabel = 'Login';
  emailPlaceholderLabel = 'Email';
  passwordPlaceholderLabel = 'Password';
  invalidLoginLabel: Observable<any>;
  invalidPasswordLabel = 'Password inválido';
  invalidEmailLabel = 'Email inválido';

  constructor(private formBuilder: FormBuilder,
              private routerService: RouterService,
              private authService: AuthService) { }

  ngOnInit(): void {
    this.returnUrl = this.rootPath;
    this.invalidLoginLabel = this.authService.errorMessage$;

    this.form = this.formBuilder.group({
      username: ['', Validators.email],
      password: ['', Validators.required]
    });

    // TODO: Move to Guard
    if (this.authService.checkAuthenticated()) {
      this.routerService.navigateTo(this.rootPath);
    }
  }

  onSubmit(): void {
    const loginModel: LoginModel = {
      email: this.form.get('username').value,
      password: this.form.get('password').value
    };
    this.authService.login(loginModel).subscribe(() => {
      this.routerService.navigateTo(this.rootPath);
    });
  }
}
