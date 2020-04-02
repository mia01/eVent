import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import Task from 'ClientApp/app/models/task';
import Priority from 'ClientApp/app/models/priority';
import IAddTaskForm from 'ClientApp/app/interfaces/addTaskForm';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.css']
})
export class AddTaskComponent implements OnInit {

  form: FormGroup;
  priorities: Priority[];
  task: Task;

  constructor(
    private formBuilder: FormBuilder,
    private modal: MatDialogRef<AddTaskComponent>,
    @Inject(MAT_DIALOG_DATA) private data: IAddTaskForm
  ) {
    this.task = data.taskData;
    this.priorities = data.priorities;
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      id: this.task ? this.task.id : null,
      title: [this.task ? this.task.title : "", Validators.required],
      description: this.task ? this.task.description : "",
      dueDate: [this.task ? this.task.dueDate : "", Validators.required],
      priorityId: [this.task ? this.task.priorityId : "", Validators.required],
    })
  }

  submit(form) {
    if (form.valid) {
      this.modal.close(form.value as Task);
    }
  }
}
