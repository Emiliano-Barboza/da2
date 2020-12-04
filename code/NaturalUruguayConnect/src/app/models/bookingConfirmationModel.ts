import { LodgmentOptionsModel } from './lodgmentOptionsModel';
export interface BookingConfirmationModel {
  name: string;
  lastName: string;
  email: string;
  lodgmentOptions: LodgmentOptionsModel;
}
