import { Component, OnInit, Input } from '@angular/core';
import { InvalidIdInfo } from '../models/InvalidIdInfo';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-home-invalidId',
  templateUrl: './home-invalidId.component.html',
  styleUrls: ['./home-invalidId.component.css']
})
export class HomeInvalidIdComponent implements OnInit {
  @Input() invalidIdInfos: InvalidIdInfo[];
  page = 1;
  constructor() { }

  ngOnInit() {
  }

}
