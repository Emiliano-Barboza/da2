import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {Lodgment} from '../../models/lodgments';
import {SelectionModel} from '@angular/cdk/collections';
import {MatSort} from '@angular/material/sort';
import {RouterService} from '../../services/router/router.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Review} from '../../models/review';
import {HttpParams} from '@angular/common/http';

@Component({
  selector: 'app-reviews',
  templateUrl: './reviews.component.html',
  styleUrls: ['./reviews.component.css']
})
export class ReviewsComponent implements OnInit {

  reviewData: FormGroup;
  confirmationCodeLabel = 'Código de reserva';
  confirmationCodePlaceholderLabel = 'Ingrese código de reserva';
  invalidConfirmationCodeLabel = 'Código de reserva inválido';
  amountOfStarsLabel = 'Cantidad de estrellas';
  amountOfStarsPlaceholderLabel = 'Ingrese cantidad de estrellas';
  invalidAmountOfStarsLabel = 'Cantidad de estrellas inválido';
  commentLabel = 'Reseña';
  commentPlaceholderLabel = 'Ingrese reseña';
  invalidCommentLabel = 'Reseña inválida';
  createReviewLabel = 'Crear Reseña';
  justifyContentHeader = 'flex-end';
  filterByPlaceholder = 'Filtrar por reseña';
  filterBy = '';
  total: number;
  pageSize: number;
  displayedColumns: string[] = ['name', 'amountOfStars', 'comment'];
  columnsToDisplay: string[] = this.displayedColumns.slice();
  selection = new SelectionModel<Lodgment>(false, []);

  @ViewChild(MatSort) sort: MatSort;
  @Input() reviews: Review[] = [];
  @Output() reviewEvent = new EventEmitter<Review>();
  @Output() searchEvent = new EventEmitter<HttpParams>();

  constructor(private formBuilder: FormBuilder, private routerService: RouterService) {}

  ngOnInit(): void {
    this.reviewData = this.formBuilder.group({
      confirmationCode: ['', Validators.required],
      amountOfStars: ['', [Validators.required, Validators.max(5), Validators.min(1)]],
      comment: ['', Validators.required]
    });
  }

  filterByEvent(filterBy): void {
    this.filterBy = filterBy;
    this.search();
  }

  search(pageEvent = null): void {
    const pagingModel = this.routerService.getPagingModel(this.sort.direction, this.sort.active, this.filterBy, pageEvent);
    const params = this.routerService.buildPagingParams(pagingModel);
    this.searchEvent.emit(params);
  }

  sortBy(): void {
    this.search();
  }

  createReview(): void {
    this.reviewData.markAllAsTouched();
    if (this.reviewData.valid) {
      const review: Review = {
        amountOfStars: this.reviewData.get('amountOfStars').value,
        comment: this.reviewData.get('comment').value,
        confirmationCode: this.reviewData.get('confirmationCode').value
      };
      this.reviewEvent.emit(review);
    }
  }

  pageChangeEvent(pageEvent): void {
    this.search(pageEvent);
  }
}
