import { Component, OnInit } from '@angular/core';
import { TasksService } from '../services/tasks.service';
import Task from '../models/task';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AddTaskComponent } from './add/add.component';
import { PriorityService } from '../services/priority.service';
import Priority from '../models/priority';
import IAddTaskForm from '../interfaces/addTaskForm';
import { FriendsService } from '../services/friends.service';
import UserFriendResponse from '../models/UserFriendResponse';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss']
})
export class TasksComponent implements OnInit {

  addTaskModal: MatDialogRef<AddTaskComponent>;
  public tasks: Task[] = [];
  public priorities: Priority[] = [];
  public friends: UserFriendResponse[] = [];

  constructor(
    private taskService: TasksService,
    private friendService: FriendsService,
    private priorityService: PriorityService,
    private modal: MatDialog
  ) { }

  async ngOnInit() {
    this.priorities = await this.priorityService.getAllPriorities();
    this.tasks = await this.taskService.getAllTasks();
    this.friends = await this.friendService.getUserFriends();
  }

  updateTaskDoneStatus(event: MatCheckboxChange, task: Task) {
    task.done = event.checked;
    this.createOrUpdateTask(task, null);
  }

  async openAddTaskModal(task?: Task) {
    let priorities = this.priorities;
    let friends = this.friends;
    let taskData = task || {};
    this.addTaskModal = this.modal.open(AddTaskComponent, {
      data: { taskData, priorities, friends } as IAddTaskForm,
      disableClose: true,
      autoFocus : true
    });

    let formData = await this.addTaskModal.afterClosed().toPromise();
    await this.createOrUpdateTask(formData, task);
  }

  async deleteTask(task: Task) {
    var response = await this.taskService.deleteTask(task);
    console.log(response);
    let taskIndex = this.tasks.indexOf(task);
    this.tasks.splice(taskIndex, 1);
  }

  private async createOrUpdateTask(taskData: any, task?: Task) {
    if (taskData.id) {
      let updatedTask = await this.taskService.updateTask(taskData);
      let taskIndex = this.tasks.indexOf(task);
      this.tasks[taskIndex] = updatedTask;
    } else if (taskData) {
      let newTask = await this.taskService.addTask(taskData);
      this.tasks.push(newTask);
    }
  }
}
