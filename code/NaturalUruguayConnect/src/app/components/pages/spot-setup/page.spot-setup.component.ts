import {Component, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Observable} from 'rxjs';
import {SpotsService} from '../../../services/spots/spots.service';
import {ModalService} from '../../../services/modal/modal.service';
import {RouterService} from '../../../services/router/router.service';
import {AuthService} from '../../../services/auth/auth.service';
import {MatDialogConfig} from '@angular/material/dialog';
import {ErrorDialogComponent} from '../../dialogs/error-dialog/error-dialog.component';
import {PageEvent} from '@angular/material/paginator';
import {RegionsService} from '../../../services/regions/regions.service';
import {Spot} from '../../../models/spot';
import {environment} from '../../../../environments/environment';

@Component({
  selector: 'app-spot-setup',
  templateUrl: './page.spot-setup.component.html',
  styleUrls: ['./page.spot-setup.component.css']
})
export class PageSpotSetupComponent implements OnInit {

  goBackUrl = '/spots';
  goBackUrlName = 'Volver a puntos turísticos';
  justifyContentHeader = 'space-between';
  isLinear = true;
  isOptional = true;
  informationData: FormGroup;
  imagesData: FormGroup;
  informationLabel = 'Datos del punto turístico';
  imagesLabel = 'Imagenes del punto turístico';
  nameLabel = 'Nombre';
  namePlaceholderLabel = 'Nombre';
  invalidNameLabel = 'El nombre no puede ser vacío.';
  regionPlaceholderLabel = '';
  invalidRegionLabel = 'La región no puede ser vacía.';
  descriptionLabel = 'Descripción';
  descriptionPlaceholderLabel = 'Descripción';
  invalidDescriptionLabel = 'La descripción no puede ser vacía.';
  createSpotLabel = 'Crear punto turístico';
  spot = undefined;
  selectRegionPlaceholderText = 'Ingresa la región';
  filteredRegions: Observable<any>;
  spotImagesUrl = '';
  token =  '';
  baseUrl =  '';

  @ViewChild('stepper') stepper;

  constructor(private formBuilder: FormBuilder, private spotService: SpotsService,
              private modalService: ModalService, private routerService: RouterService,
              private authService: AuthService, private regionsService: RegionsService) {
    this.baseUrl = environment.apiUrl;
  }

  ngOnInit() {
    // TODO: Move to Guard
    if (!this.authService.checkAuthenticated()) {
      this.routerService.navigateTo('/');
    }
    this.filteredRegions = this.regionsService.regions$;
    this.informationData = this.formBuilder.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      regionId: ['', Validators.required]
    });
    this.imagesData = this.formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
  }

  createSpot(): void {
    this.informationData.markAllAsTouched();
    if (this.informationData.valid) {
      this.informationData.disable();
      const spot: Spot = {
        name: this.informationData.get('name').value,
        description: this.informationData.get('description').value,
        thumbnail: ''
      };
      const regionIdId =  this.informationData.get('regionId').value;
      this.regionsService.createSpot(regionIdId, spot).subscribe((spotReponse) => {
        this.isLinear = false;
        this.spot = spotReponse;
        this.token = this.authService.getToken();
        // TODO: move this to some service
        this.spotImagesUrl =  this.baseUrl + 'spots/' + this.spot.id + '/images';
      }, (error) => {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.data = error;
        this.stepper.previous();
        this.informationData.enable();
        this.modalService.open(ErrorDialogComponent, dialogConfig);
      });
    }
  }

  searchSource(value): void {
    if (value){
      const pageEvent = new PageEvent();
      pageEvent.length = 100;
      const pagingModel = this.routerService.getPagingModel('', '', value, pageEvent);
      const params = this.routerService.buildPagingParams(pagingModel);
      this.regionsService.getRegions(params);
    }
  }

  selectedRegion(region): void {
    this.informationData.patchValue({
      regionId: region.id
    });
  }
}
