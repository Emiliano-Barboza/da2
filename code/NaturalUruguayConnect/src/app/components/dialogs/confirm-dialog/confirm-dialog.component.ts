import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit {
  titleLabel = 'Confirmación';
  closeLabel = 'Cerrar';
  confirmLabel = 'Confirmar';
  message = '';

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<ConfirmDialogComponent>) {
    this.message = data.context;
  }

  ngOnInit(): void {
  }

  close(): void {
    this.dialogRef.close();
  }
}
