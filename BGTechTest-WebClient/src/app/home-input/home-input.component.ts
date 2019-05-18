import { IdNumbersDto } from './../models/IdNumbersDto';
import { IdInfo } from './../models/IdInfo';
import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { IdentityNumberServiceService } from '../services/IdentityNumberService.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { analyzeAndValidateNgModules } from '@angular/compiler';

@Component({
  selector: 'app-home-input',
  templateUrl: './home-input.component.html',
  styleUrls: ['./home-input.component.css']
})
export class HomeInputComponent implements OnInit {
  idNumbersFormGroup: FormGroup;
  idNumbersDto: IdNumbersDto;
  @Output() RefreshIdInfo = new EventEmitter();
  uploader: FileUploader;
  baseUrl = environment.appUrl;
  hasBaseDropZoneOver = false;

  constructor(private identityNumberService: IdentityNumberServiceService, private formbuilder: FormBuilder,
              private toastr: ToastrService) {
    this.idNumbersFormGroup = this.createidNumbersFormGroupBuilder(formbuilder);
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'IdentityNumber/uploadcsv',
      isHTML5: true,
      allowedMimeType: ['text/plain'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 5 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };
    // callback fuction after each item has been uploaded
    this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {
      if (status === 204) {
        this.toastr.success('Successfully uploaded file: ' + item.file.name);
        this.RefreshIdInfo.emit(true);
      } else if (status === 400) {
        this.toastr.error('Invalid file format for file: ' + item.file.name);
      } else {
        this.toastr.error('The file: ' + item.file.name + ' was not uploaded');
      }
    };
  }
  onFormSubmit() {
    if (this.idNumbersFormGroup.valid) {
      const idNumbersRequest = Object.assign({}, this.idNumbersFormGroup.value);
      this.idNumbersDto = new IdNumbersDto();
      this.idNumbersDto.IdNumbers = idNumbersRequest.idNumbers;
      // post numbers to api
      this.addIdNumbers(this.idNumbersDto);
    }
  }

  addIdNumbers(model: IdNumbersDto) {
    this.identityNumberService.addIdentityNumbers(model).subscribe(() => {
      this.toastr.success('Successfully parsed identity numbers');
      this.RefreshIdInfo.emit(true);
      // clear form data
      this.idNumbersFormGroup.reset();
    }, error => {
      this.toastr.error('Identity numbers were not parsed');
    });
  }
  createidNumbersFormGroupBuilder(formbuilder: FormBuilder) {
    return formbuilder.group({
      idNumbers: ['', Validators.required]
    });
  }
  ngOnInit() {
    this.initializeUploader();
  }

}
