import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

import Event from '../models/event';
import { map } from 'rxjs/operators';
import UserEvent from '../models/adapters/event';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  private EVENT_URL = environment.api.base + environment.api.events;

  constructor(
    private httpClient: HttpClient
  ) { }

  public getAllEvents(): Promise<Event[]> {
      return this.httpClient.get<Event[]>(this.EVENT_URL).toPromise();
  }
  
  public getAllAddaptedEvents(): Promise<UserEvent[]> {
    return this.httpClient.get<Event[]>(this.EVENT_URL).pipe(
      map(events => events.map(event => new UserEvent(event)))
    ).toPromise();
  }

  public addEvent(event: Event): Promise<any> {
    return this.httpClient.post(this.EVENT_URL, event).toPromise();
  }

  public updateEvent(event: Event): Promise<any> {
    return this.httpClient.post(this.EVENT_URL + '/' + event.id, event).toPromise();
  } 
  
  public deleteEvent(event: Event): Promise<any> {
    return this.httpClient.delete(this.EVENT_URL + '/' + event.id).toPromise();
  }
}
