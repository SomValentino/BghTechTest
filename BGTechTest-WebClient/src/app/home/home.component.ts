import { ValidIdInfo } from './../models/ValidIdInfo';
import { Component, OnInit } from '@angular/core';
import { IdentityNumberServiceService } from '../services/IdentityNumberService.service';
import { InvalidIdInfo } from '../models/InvalidIdInfo';
import { IdInfo } from '../models/IdInfo';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  idInfo: IdInfo;
  constructor(private identityNumberService: IdentityNumberServiceService) {
    this.getIdInformation();
  }
  refreshIdInfo(shouldRefresh: boolean) {
    if (shouldRefresh) {
      this.getIdInformation();
    }
  }
  getIdInformation() {
    this.identityNumberService.getIdentityNumbers().subscribe((idInfo: IdInfo) => {
      this.idInfo = idInfo;
    });
  }
  ngOnInit() {
  }

}
