import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';

@Component({
  selector: 'app-error-dialog',
  templateUrl: './error-dialog.component.html',
  styleUrls: ['./error-dialog.component.css']
})
export class ErrorDialogComponent implements OnInit {
  titleLabel = 'Error';
  closeLabel = 'Cerrar';
  message = '';

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<ErrorDialogComponent>) {
    // TODO: Improve error format
    this.message = JSON.stringify(data.context.error);
  }

  ngOnInit(): void {
  }

  close(): void {
    this.dialogRef.close();
  }
}
