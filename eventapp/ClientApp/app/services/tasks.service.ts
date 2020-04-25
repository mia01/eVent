import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

import Task from '../models/task';
import TaskEvent from '../models/adapters/task';
import { map, flatMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TasksService {

  private TASK_URL = environment.api.base + environment.api.tasks;

  constructor(
    private httpClient: HttpClient
  ) { }

  public getAllTasks(): Promise<Task[]> {
    return this.httpClient.get<Task[]>(this.TASK_URL).toPromise();
  }
  
  public getTasksAsEvents(): Promise<TaskEvent[]> {
    return this.httpClient.get<Task[]>(this.TASK_URL).pipe(
      map(tasks => tasks.map( task => new TaskEvent(task)))
    ).toPromise();
  }

  public addTask(task: Task): Promise<any> {
    return this.httpClient.post(this.TASK_URL, task).toPromise();
  }

  public updateTask(task: Task): Promise<any> {
    return this.httpClient.post(this.TASK_URL + '/' + task.id, task).toPromise();
  } 
  
  public deleteTask(task: Task): Promise<any> {
    return this.httpClient.delete(this.TASK_URL + '/' + task.id).toPromise();
  }
}
