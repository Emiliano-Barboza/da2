import {Component, ViewChild} from '@angular/core';
import {UserService} from '../../../services/users/user.service';
import {User} from '../../../models/user';
import {BaseComponent} from '../../../helpers/baseComponent';
import {takeUntil} from 'rxjs/operators';
import {RouterService} from '../../../services/router/router.service';
import {MatSort} from '@angular/material/sort';
import {SelectionModel} from '@angular/cdk/collections';
import {ModalService} from '../../../services/modal/modal.service';
import {MatDialogConfig} from '@angular/material/dialog';
import {ConfirmDialogComponent} from '../../dialogs/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-users',
  templateUrl: './page.users.component.html',
  styleUrls: ['./page.users.component.css']
})
export class PageUsersComponent extends BaseComponent {
  addUserLabel = 'Agregar';
  modifyUserLabel = 'Modificar';
  deleteUserLabel = 'Borrar';
  deleteUserWarningLabel = 'Seguro que quieres borrar el usuario?';
  justifyContentHeader = 'flex-end';
  filterByPlaceholder = 'Filtrar por usuarios';
  filterBy = '';
  total: number;
  pageSize: number;
  displayedColumns: string[] = ['id', 'name', 'email'];
  columnsToDisplay: string[] = this.displayedColumns.slice();
  users: User[];
  selection = new SelectionModel<User>(false, []);
  userOptionsDisabled: boolean;
  userDeletedSuccessLabel = 'Usuario borrado con éxito';
  userErrorLabel = 'No se pudo borrar el usuario';

  @ViewChild(MatSort) sort: MatSort;

  constructor(private userService: UserService, private routerService: RouterService, private modalService: ModalService) {
    super();
  }

  private deleteUser(user: User): void {
    this.userService.deleteUser(user).subscribe(() => {
      this.modalService.openSuccessSnackBar(this.userDeletedSuccessLabel);
      this.searchUsers();
    }, (error) => {
      this.modalService.openErrorSnackBar(this.userErrorLabel);
    });
  }

  ngOnInit(): void {
    this.userOptionsDisabled = true;
    this.userService.users$.pipe(takeUntil(this.destroyed)).subscribe(users => this.users = users);
    this.userService.total$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.total = x;
    });
    this.userService.pageSize$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.pageSize = x;
    });
    this.selection.changed.pipe(takeUntil(this.destroyed)).subscribe(() => {
      this.userOptionsDisabled = !this.selection.hasValue();
    });
  }

  filterByUser(filterBy): void {
    this.filterBy = filterBy;
    this.searchUsers();
  }

  searchUsers(pageEvent = null): void {
    const pagingModel = this.routerService.getPagingModel(this.sort.direction, this.sort.active, this.filterBy, pageEvent);
    const params = this.routerService.buildPagingParams(pagingModel);
    const queryParams = this.routerService.buildQueryParams(params);
    this.userService.getUsers(params);
    this.routerService.refreshQueryParams(queryParams);
  }

  sortBy(): void {
    this.searchUsers();
  }

  addUser(): void {
    const url = 'users/create';
    this.routerService.navigateTo(url);
  }

  modifyUser(): void {
    const url = 'users/' + this.selection.selected[0].id +  '/modify';
    this.routerService.navigateTo(url);
  }

  confirmDeleteUser(): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data = this.deleteUserWarningLabel;
    this.modalService.open(ConfirmDialogComponent, dialogConfig).subscribe(result => {
      if (result) {
        this.deleteUser(this.selection.selected[0]);
      }
    });
  }

  pageChangeEvent(pageEvent): void {
    this.searchUsers(pageEvent);
  }
}
