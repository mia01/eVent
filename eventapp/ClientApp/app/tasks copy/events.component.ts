import { Component, OnInit } from '@angular/core';
import Event from '../models/event';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AddEventComponent } from './add/add.component';
import IAddEventForm from '../interfaces/addEventForm';
import { EventService } from '../services/event.service';
import { AuthorizeService } from '../services/auth/authorize.service';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss']
})
export class EventsComponent implements OnInit {

  addEventModal: MatDialogRef<AddEventComponent>;
  public username: string;
  public events: Event[] = [];

  constructor(
    private authService: AuthorizeService,
    private eventService: EventService,
    private modal: MatDialog
  ) { }

  async ngOnInit() {
    this.authService.getUser().subscribe(u => {
      this.username = u.name;
    });
    this.events = await this.eventService.getAllEvents();
  }

  async openAddEventModal(event?: Event) {
    let eventData = event || {};
    this.addEventModal = this.modal.open(AddEventComponent, {
      data: { eventData: eventData } as IAddEventForm,
      disableClose: true,
      autoFocus : true
    });

    let formData = await this.addEventModal.afterClosed().toPromise();
    await this.createOrUpdateEvent(formData, event);
  }

  async deleteEvent(event: Event) {
    var response = await this.eventService.deleteEvent(event);
    let eventIndex = this.events.indexOf(event);
    this.events.splice(eventIndex, 1);
  }

  private async createOrUpdateEvent(eventData: any, event?: Event) {
    if (eventData.id) {
      let updatedEvent = await this.eventService.updateEvent(eventData);
      let eventIndex = this.events.indexOf(event);
      this.events[eventIndex] = updatedEvent;
    } else if (eventData) {
      let newEvent = await this.eventService.addEvent(eventData);
      this.events.push(newEvent);
    }
  }
}
