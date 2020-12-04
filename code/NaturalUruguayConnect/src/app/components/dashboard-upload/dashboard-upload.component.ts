import {Component, Input, OnInit} from '@angular/core';
import {Uppy} from '@uppy/core';
import * as Dashboard from '@uppy/dashboard';
import * as XHRUpload from '@uppy/xhr-upload';

@Component({
  selector: 'app-dashboard-upload',
  templateUrl: './dashboard-upload.component.html',
  styleUrls: ['./dashboard-upload.component.css']
})
export class DashboardUploadComponent implements OnInit {
  @Input() uploadUrl = '';
  @Input() token = '';

  constructor() { }

  ngOnInit(): void {
    const self = this;
    window.setTimeout(function (){
      console.log(self);
      const allowedFormats = ['image/*'];
      const maxFileSizeInMB = 2;
      const bytesToMB = 1000000;
      const uppy = new Uppy({
        debug: true,
        autoProceed: false,
        restrictions: {
          maxFileSize: maxFileSizeInMB * bytesToMB,
          allowedFileTypes: allowedFormats
        }
      })
        .use(Dashboard, {
          trigger: '#select-files',
          inline: true,
          target: '.dashboard-container',
          replaceTargetContent: true,
          showProgressDetails: true,
          note: 'Solamente imagenes hasta 2 MB',
          height: 470,
          browserBackButtonClose: true
        })
        .use(XHRUpload, {
          endpoint: self.uploadUrl,
          fieldName: 'files',
          formData: true,
          headers: {
            authorization: `Bearer ${self.token}`
          }});

      uppy.on('complete', (result) => {
        console.log('Upload complete! We’ve uploaded these files:', result.successful);
      });
    }, 1000);
  }
}
