import { Spot } from './spot';
export interface Lodgment {
  id?: number;
  name?: string;
  description?: string;
  address?: string;
  phoneNumber?: string;
  contactInformation?: string;
  amountOfStars?: number;
  images?: string[];
  price?: number;
  totalPrice?: number;
  isActive?: boolean;
  spot?: Spot;
}
