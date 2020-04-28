import { Component, OnInit } from '@angular/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import { TasksService } from '../services/tasks.service';
import { EventType } from '../models/enums/EventTypes.enum';
import Task from '../models/task';
import { EventService } from '../services/event.service';
@Component({
  selector: 'app-calender',
  templateUrl: './calender.component.html',
  styleUrls: ['./calender.component.scss']
})
export class CalenderComponent implements OnInit {
  calendarPlugins = [dayGridPlugin];
  public eventSources: Array<any> = [];
  constructor(private taskService: TasksService, private eventService: EventService) { }

  ngOnInit(): void {
    this.eventSources = [
      this.fetchTasks.bind(this),
      this.fetchEvents.bind(this)
    ]
  }

  private async fetchTasks(fetchInfo, successCallback, failureCallback): Promise<void> {
    await this.taskService.getTasksAsEvents().then((events) => {
      successCallback(events);
    }).catch((error) => {
      failureCallback(error);
    });
  }
  
  private async fetchEvents(fetchInfo, successCallback, failureCallback): Promise<void> {
    await this.eventService.getAllAddaptedEvents().then((events) => {
      successCallback(events);
    }).catch((error) => {
      failureCallback(error);
    });
  }

  public eventRender(info: any) {
    if (info.event.extendedProps.type == EventType.Task) {

      let task = info.event.extendedProps.task as Task;

      let element = info.el.querySelector('.fc-content');

      var i = document.createElement('i');
      i.className = task.done? 'fa fa-check-square-o' : 'fa fa-square-o';

      element.prepend(i);

      if (task.priorityId == 1) {
        info.el.classList.add("medium-priority");
      } else if (task.priorityId == 2) {
        info.el.classList.add("high-priority");
      } else if (task.priorityId == 3) {
        info.el.classList.add("low-priority");
      }
    };

    console.log("rendering");
    console.log(info);
  }
}
