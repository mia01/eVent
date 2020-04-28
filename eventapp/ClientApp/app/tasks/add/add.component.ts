import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import Task from 'ClientApp/app/models/task';
import Priority from 'ClientApp/app/models/priority';
import IAddTaskForm from 'ClientApp/app/interfaces/addTaskForm';
import UserFriendResponse from 'ClientApp/app/models/UserFriendResponse';
@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddTaskComponent implements OnInit {

  form: FormGroup;
  priorities: Priority[];
  friends: UserFriendResponse[];
  task: Task;

  constructor(
    private formBuilder: FormBuilder,
    private modal: MatDialogRef<AddTaskComponent>,
    @Inject(MAT_DIALOG_DATA) private data: IAddTaskForm
  ) {
    this.task = data.taskData;
    this.priorities = data.priorities;
    this.friends = data.friends;
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      id: this.task ? this.task.id : null,
      title: [this.task ? this.task.title : "", Validators.required],
      description: this.task ? this.task.description : "",
      dueDate: [this.task ? new Date(this.task.dueDate + "Z") : "", Validators.required],
      priorityId: [this.task ? this.task.priorityId : "", Validators.required],
      assignedTo: [this.task ? this.task.assignedTo : ""],
      reminder: [this.task ? this.task.reminder : false],
    })
  }

  submit(form) {
    if (form.valid) {
      var task = form.value as Task;
      if (task.reminder == null) {
        task.reminder = false;
      }
      this.modal.close(task);
    }
  }
}
