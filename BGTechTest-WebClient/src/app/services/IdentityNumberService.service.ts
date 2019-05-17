import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { IdNumbersDto } from '../models/IdNumbersDto';
import { Observable } from 'rxjs';
import { IdInfo } from '../models/IdInfo';

@Injectable({
  providedIn: 'root'
})
export class IdentityNumberServiceService {

baseUrl = environment.appUrl;
constructor(private httpClient: HttpClient) { }

addIdentityNumbers(body: IdNumbersDto) {
  return this.httpClient.post(this.baseUrl + 'IdentityNumber/addids', body);
}

getIdentityNumbers(): Observable<IdInfo> {
  return this.httpClient.get<IdInfo>(this.baseUrl + 'IdentityNumber/getIds');
}

}
