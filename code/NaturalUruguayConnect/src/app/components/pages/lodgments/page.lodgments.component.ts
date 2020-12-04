import {Component, OnInit, ViewChild} from '@angular/core';
import {SelectionModel} from '@angular/cdk/collections';
import {MatSort} from '@angular/material/sort';
import {RouterService} from '../../../services/router/router.service';
import {takeUntil} from 'rxjs/operators';
import {BaseComponent} from '../../../helpers/baseComponent';
import {Lodgment} from '../../../models/lodgments';
import {LodgmentService} from '../../../services/lodgment/lodgment.service';
import {MatDialogConfig} from '@angular/material/dialog';
import {ConfirmDialogComponent} from '../../dialogs/confirm-dialog/confirm-dialog.component';
import {ModalService} from '../../../services/modal/modal.service';

@Component({
  selector: 'app-lodgments',
  templateUrl: './page.lodgments.component.html',
  styleUrls: ['./page.lodgments.component.css']
})
export class PageLodgmentsComponent extends BaseComponent {

  addLodgmentLabel = 'Agregar';
  activateLodgmentLabel = 'Activar';
  deactivateLodgmentLabel = 'Desactivar';
  toggleLodgmentLabel = 'Desactivar';
  deleteLodgmentLabel = 'Borrar';
  justifyContentHeader = 'flex-end';
  filterByPlaceholder = 'Filtrar por hospedajes';
  deleteWarningLabel = 'Seguro que quieres borrar el hospedaje?';
  deleteSuccessLabel = 'Hospedaje borrado con éxito';
  activateSuccessLabel = 'Activado con éxito';
  deactivateSuccessLabel = 'Desactivado con éxito';
  errorLabel = 'No se pudo borrar el hospedaje';
  filterBy = '';
  total: number;
  pageSize: number;
  displayedColumns: string[] = ['id', 'name', 'address', 'amountOfStars', 'capacity', 'contactInformation', 'description', 'price', 'isActive', 'phoneNumber'];
  columnsToDisplay: string[] = this.displayedColumns.slice();
  lodgments: Lodgment[];
  selection = new SelectionModel<Lodgment>(false, []);
  optionsDisabled: boolean;

  @ViewChild(MatSort) sort: MatSort;

  constructor(private lodgmentService: LodgmentService, private routerService: RouterService, private modalService: ModalService) {
    super();
  }

  private updateToggleLabel(): void {
    if (this.selection.hasValue()) {
      this.toggleLodgmentLabel = this.selection.selected[0].isActive ? this.deactivateLodgmentLabel : this.activateLodgmentLabel;
    } else {
      this.toggleLodgmentLabel = this.deactivateLodgmentLabel;
    }
  }

  private delete(): void {
    const lodgment = this.selection.selected[0];
    this.lodgmentService.deleteLodgment(lodgment).subscribe(() => {
      this.modalService.openSuccessSnackBar(this.deleteSuccessLabel);
      this.search();
    }, (error) => {
      this.modalService.openErrorSnackBar(this.errorLabel);
    });
  }

  private activate(): void {
    const lodgment = this.selection.selected[0];
    this.lodgmentService.activateLodgment(lodgment).subscribe(() => {
      this.modalService.openSuccessSnackBar(this.activateSuccessLabel);
      this.selection.selected[0].isActive = true;
      this.updateToggleLabel();
    }, (error) => {
      this.modalService.openErrorSnackBar(this.errorLabel);
    });
  }

  private deactivate(): void {
    const lodgment = this.selection.selected[0];
    this.lodgmentService.deactivateLodgment(lodgment).subscribe(() => {
      this.modalService.openSuccessSnackBar(this.deactivateSuccessLabel);
      this.selection.selected[0].isActive = false;
      this.updateToggleLabel();
    }, (error) => {
      this.modalService.openErrorSnackBar(this.errorLabel);
    });
  }

  ngOnInit(): void {
    this.optionsDisabled = true;
    this.lodgmentService.lodgments$.pipe(takeUntil(this.destroyed)).subscribe(lodgments => this.lodgments = lodgments);
    this.lodgmentService.total$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.total = x;
    });
    this.lodgmentService.pageSize$.pipe(takeUntil(this.destroyed)).subscribe(x => {
      this.pageSize = x;
    });
    this.selection.changed.pipe(takeUntil(this.destroyed)).subscribe(() => {
      this.optionsDisabled = !this.selection.hasValue();
      this.updateToggleLabel();
    });
  }

  filterByEvent(filterBy): void {
    this.filterBy = filterBy;
    this.search();
  }

  search(pageEvent = null): void {
    const pagingModel = this.routerService.getPagingModel(this.sort.direction, this.sort.active, this.filterBy, pageEvent);
    const params = this.routerService.buildPagingParams(pagingModel);
    const queryParams = this.routerService.buildQueryParams(params);
    this.lodgmentService.getLodgments(params);
    this.routerService.refreshQueryParams(queryParams);
  }

  sortBy(): void {
    this.search();
  }

  createLodgment(): void {
    const url = 'lodgments/create';
    this.routerService.navigateTo(url);
  }

  pageChangeEvent(pageEvent): void {
    this.search(pageEvent);
  }

  confirmDelete(): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data = this.deleteWarningLabel;
    this.modalService.open(ConfirmDialogComponent, dialogConfig).subscribe(result => {
      if (result) {
        this.delete();
      }
    });
  }

  toggleLodgment(): void {
    if (this.selection.selected[0].isActive) {
      this.deactivate();
    } else {
      this.activate();
    }
  }
}
