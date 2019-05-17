import { ValidIdInfo } from './../models/ValidIdInfo';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-home-validId',
  templateUrl: './home-validId.component.html',
  styleUrls: ['./home-validId.component.css']
})
export class HomeValidIdComponent implements OnInit {
  @Input() validIdInfos: ValidIdInfo[];
  page = 1;
  constructor() { }
  ngOnInit() {
  }

}
