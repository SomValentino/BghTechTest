import { IdNumbersDto } from './../models/IdNumbersDto';
import { IdInfo } from './../models/IdInfo';
import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { IdentityNumberServiceService } from '../services/IdentityNumberService.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home-input',
  templateUrl: './home-input.component.html',
  styleUrls: ['./home-input.component.css']
})
export class HomeInputComponent implements OnInit {
  idNumbersFormGroup: FormGroup;
  idNumbersDto: IdNumbersDto;
  @Output() RefreshIdInfo = new EventEmitter();
  constructor(private identityNumberService: IdentityNumberServiceService, private formbuilder: FormBuilder,
              private toastr: ToastrService) {
    this.idNumbersFormGroup = this.createidNumbersFormGroupBuilder(formbuilder);
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
  }

}
