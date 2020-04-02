import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import Priority from '../models/priority';

@Injectable({
  providedIn: 'root'
})
export class PriorityService {

  private PRIORITY_URL = environment.api.base + environment.api.priorities;

  constructor(
    private httpClient: HttpClient
  ) { }


  public getAllPriorities(): Promise<Priority[]> {
    return this.httpClient.get<Priority[]>(this.PRIORITY_URL).toPromise();
  }
}
