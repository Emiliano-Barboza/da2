import {PagingModel} from './pagingModel';

export interface PaginatedCountModel {
  total: number;
  pages: number;
  currentPage: number;
  nextPage: string;
  previousPage: string;
  paging: PagingModel;
}
