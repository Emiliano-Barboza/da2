import { Region } from './region';

export interface Spot {
  id?: number;
  name?: string;
  description?: string;
  thumbnail?: string;
  region?: Region;
}
