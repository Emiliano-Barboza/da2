import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {UserService} from '../../../services/users/user.service';
import {User} from '../../../models/user';
import {AuthService} from '../../../services/auth/auth.service';
import {RouterService} from '../../../services/router/router.service';
import {ModalService} from '../../../services/modal/modal.service';

@Component({
  selector: 'app-add-user',
  templateUrl: './page.add-user.component.html',
  styleUrls: ['./page.add-user.component.css']
})
export class PageAddUserComponent implements OnInit {
  goBackUrl = '/users';
  goBackUrlName = 'Volver a usuarios';
  justifyContentHeader = 'space-between';
  form: FormGroup;
  namePlaceholderLabel = 'Nombre completo';
  invalidNameLabel = 'El nombre no puede ser vacío.';
  emailPlaceholderLabel = 'Email';
  invalidEmailLabel = 'Email inválido';
  createLabel = 'Crear';
  userCreatedSuccessLabel = 'Usuario creado con éxito';
  userErrorLabel = 'No se pudo crear el usuario';

  constructor(private userService: UserService, private formBuilder: FormBuilder, private authService: AuthService,
              private routerService: RouterService, private modalService: ModalService) { }

  ngOnInit(): void {
    // TODO: Move to Guard
    if (!this.authService.checkAuthenticated()) {
      this.routerService.navigateTo('/');
    }
    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      email: ['', Validators.email]
    });
  }

  createUser(): void {
    if (this.form.valid) {
      const user: User = {
        name: this.form.get('username').value,
        email: this.form.get('email').value
      };
      this.userService.createUser(user).subscribe(() => {
        this.modalService.openSuccessSnackBar(this.userCreatedSuccessLabel);
        this.form.reset();
      }, (error) => {
        this.modalService.openErrorSnackBar(this.userErrorLabel);
      });
    }
  }
}
