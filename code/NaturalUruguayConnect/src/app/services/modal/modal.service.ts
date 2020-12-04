import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {ComponentType} from '@angular/cdk/overlay';
import {MatDialog, MatDialogConfig} from '@angular/material/dialog';
import {BookingDetailsComponent} from '../../components/dialogs/booking-details/booking-details.component';
import {MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition} from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private titleLabel = 'Título';
  private confirmLabel = 'Aceptar';
  private closeLabel = 'Cerrar';
  private timeInSeconds = 2;
  private horizontalPosition: MatSnackBarHorizontalPosition = 'center';
  private verticalPosition: MatSnackBarVerticalPosition = 'top';

  constructor(public dialog: MatDialog,
              private snackBar: MatSnackBar) { }

  private openSnackBar(message: string, action: string): void {
    this.snackBar.open(message, action, {
      duration: this.timeInSeconds * 1000,
      horizontalPosition: this.horizontalPosition,
      verticalPosition: this.verticalPosition
    });
  }

  open(component: ComponentType<any>, config: MatDialogConfig): Observable<any> {
    const data = {
      titleLabel: this.titleLabel,
      confirmLabel: this.confirmLabel,
      closeLabel: this.closeLabel,
      component,
      context: config.data
    };
    config.data = data;
    const dialogRef = this.dialog.open(component, config);
    const subscription = dialogRef.afterClosed();
    return subscription;
  }

  openSuccessSnackBar(message: string): void {
    this.openSnackBar(message, 'éxitoso');
  }

  openErrorSnackBar(message: string): void {
    this.openSnackBar(message, 'error');
  }
}
