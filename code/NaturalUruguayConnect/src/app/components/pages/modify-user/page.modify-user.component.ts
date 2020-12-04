import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {UserService} from '../../../services/users/user.service';
import {AuthService} from '../../../services/auth/auth.service';
import {RouterService} from '../../../services/router/router.service';
import {ModalService} from '../../../services/modal/modal.service';
import {User} from '../../../models/user';
import {BaseComponent} from '../../../helpers/baseComponent';

@Component({
  selector: 'app-modify-user',
  templateUrl: './page.modify-user.component.html',
  styleUrls: ['./page.modify-user.component.css']
})
export class PageModifyUserComponent extends BaseComponent {

  goBackUrl = '/users';
  goBackUrlName = 'Volver a usuarios';
  justifyContentHeader = 'space-between';
  form: FormGroup;
  namePlaceholderLabel = 'Nombre completo';
  invalidNameLabel = 'El nombre no puede ser vacío.';
  updateLabel = 'Actualizar';
  userModifiedSuccessLabel = 'Usuario modificado con éxito';
  userErrorLabel = 'No se pudo modificar el usuario';
  user: User;

  constructor(private userService: UserService, private formBuilder: FormBuilder, private authService: AuthService,
              private routerService: RouterService, private modalService: ModalService) {
    super();
    this.form = this.formBuilder.group({
      username: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    // TODO: Move to Guard
    if (!this.authService.checkAuthenticated()) {
      this.routerService.navigateTo('/');
    }
    this.userService.user$.subscribe(user => {
      this.user = user;
      this.form.setValue({
        username: this.user.name
      });
    } );
  }

  updateUser(): void {
    if (this.form.valid) {
      this.user.name = this.form.get('username').value;
      this.userService.updateUser(this.user).subscribe(() => {
        this.modalService.openSuccessSnackBar(this.userModifiedSuccessLabel);
      }, (error) => {
        this.modalService.openErrorSnackBar(this.userErrorLabel);
      });
    }
  }
}
