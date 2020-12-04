import {Component, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {SpotsService} from '../../../services/spots/spots.service';
import {Lodgment} from '../../../models/lodgments';
import {ModalService} from '../../../services/modal/modal.service';
import {ErrorDialogComponent} from '../../dialogs/error-dialog/error-dialog.component';
import {MatDialogConfig} from '@angular/material/dialog';
import {Observable} from 'rxjs';
import {RouterService} from '../../../services/router/router.service';
import {PageEvent} from '@angular/material/paginator';
import {AuthService} from '../../../services/auth/auth.service';
import {environment} from '../../../../environments/environment';

@Component({
  selector: 'app-lodgment-setup',
  templateUrl: './page.lodgment-setup.component.html',
  styleUrls: ['./page.lodgment-setup.component.css']
})
export class PageLodgmentSetupComponent implements OnInit {
  goBackUrl = '/lodgments';
  goBackUrlName = 'Volver a hospedajes';
  justifyContentHeader = 'space-between';
  isLinear = true;
  isOptional = true;
  informationData: FormGroup;
  imagesData: FormGroup;
  informationLabel = 'Datos del hospedaje';
  imagesLabel = 'Imagenes del hospedaje';
  nameLabel = 'Nombre';
  namePlaceholderLabel = 'Nombre';
  invalidNameLabel = 'El nombre no puede ser vacío.';
  descriptionLabel = 'Descripción';
  descriptionPlaceholderLabel = 'Descripción';
  invalidDescriptionLabel = 'La descripción no puede ser vacía.';
  addressLabel = 'Dirección';
  addressPlaceholderLabel = 'Dirección';
  invalidAddressLabel = 'La dirección no puede ser vacía.';
  phoneNumberLabel = 'Número de teléfono';
  phoneNumberPlaceholderLabel = 'Número de teléfono';
  invalidPhoneNumberLabel = 'El número de teléfono no puede ser vacío.';
  contactInformationLabel = 'Información de contacto';
  contactInformationPlaceholderLabel = 'Información de contacto';
  invalidContactInformationLabel = 'La información de contacto no puede ser vacío.';
  amountOfStarsLabel = 'Cantidad de estrellas';
  amountOfStarsPlaceholderLabel = 'Ingrese cantidad de estrellas';
  invalidAmountOfStarsLabel = 'La cantidad de estrellas debe ser mayor a cero.';
  priceLabel = 'Precio';
  pricePlaceholderLabel = 'Precio';
  invalidPriceLabel = 'El precio debe ser mayor a cero.';
  isActiveLabel = '';
  activeLabel = 'Activado';
  deactiveLabel = 'Desactivado';
  createLodgmentLabel = 'Crear hospedaje';
  lodgment = undefined;
  minStars = 1;
  maxStars = 5;
  selectSpotPlaceholderText = 'Ingresa el punto turístico';
  filteredSpots: Observable<any>;
  lodgmentImagesUrl = '';
  token =  '';
  baseUrl =  '';

  @ViewChild('stepper') stepper;

  constructor(private formBuilder: FormBuilder, private spotService: SpotsService,
              private modalService: ModalService, private routerService: RouterService,
              private authService: AuthService) {
    this.baseUrl = environment.apiUrl;
  }

  ngOnInit() {
    // TODO: Move to Guard
    if (!this.authService.checkAuthenticated()) {
      this.routerService.navigateTo('/');
    }
    this.filteredSpots = this.spotService.spots$;
    this.informationData = this.formBuilder.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      spotId: ['', Validators.required],
      address: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      contactInformation: ['', Validators.required],
      amountOfStars: ['', [Validators.required, Validators.max(5), Validators.min(1)]],
      price: ['', Validators.required],
      isActive: [false, Validators.required]
    });
    this.imagesData = this.formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
    this.isActiveLabel = this.deactiveLabel;
  }

  createLodgment(): void {
    this.informationData.markAllAsTouched();
    if (this.informationData.valid) {
      this.informationData.disable();
      const lodgment: Lodgment = {
        name: this.informationData.get('name').value,
        description: this.informationData.get('description').value,
        address: this.informationData.get('address').value,
        phoneNumber: this.informationData.get('phoneNumber').value,
        contactInformation: this.informationData.get('contactInformation').value,
        amountOfStars: this.informationData.get('amountOfStars').value,
        price: this.informationData.get('price').value,
        isActive: this.informationData.get('isActive').value,
        images: []
      };
      const spotId =  this.informationData.get('spotId').value;
      this.spotService.createLodgment(spotId, lodgment).subscribe((lodgmentReponse) => {
        this.isLinear = false;
        this.lodgment = lodgmentReponse;
        this.token = this.authService.getToken();
        // TODO: move this to some service
        this.lodgmentImagesUrl = this.baseUrl + 'lodgments/' + this.lodgment.id + '/images';
      }, (error) => {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.data = error;
        this.stepper.previous();
        this.informationData.enable();
        this.modalService.open(ErrorDialogComponent, dialogConfig);
      });
    }
  }

  isActiveToggle(event): void {
    this.isActiveLabel = event.checked ? this.activeLabel : this.deactiveLabel;
  }

  searchSource(value): void {
    if (value){
      const pageEvent = new PageEvent();
      pageEvent.length = 100;
      const pagingModel = this.routerService.getPagingModel('', '', value, pageEvent);
      const params = this.routerService.buildPagingParams(pagingModel);
      this.spotService.getSpots(params);
    }
  }

  selectedSpot(spot): void {
    this.informationData.patchValue({
      spotId: spot.id
    });
  }
}
