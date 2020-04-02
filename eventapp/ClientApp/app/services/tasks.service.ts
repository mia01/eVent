import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

import Task from '../models/task';

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

  public addTask(task: Task): Promise<any> {
    return this.httpClient.post(this.TASK_URL, task).toPromise();
  }

  public updateTask(task: Task): Promise<any> {
    return this.httpClient.post(this.TASK_URL + '/' + task.id, task).toPromise();
  }
}
