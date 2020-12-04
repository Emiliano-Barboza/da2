import {PaginatedCountModel} from './paginatedCountModel';
export interface PaginatedModel {
  data: any[];
  counts: PaginatedCountModel;
}
